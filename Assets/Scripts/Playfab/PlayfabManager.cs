using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance;

    [Header("Input fields")]
    public InputField emailInput;
    public InputField passwordInput;

    [Header("Text")]
    public Text messageText;
    public Text msgOfTheDay;

    [Header("First Register")]
    public GameObject firstRegisterPanel;
    public InputField firstEmailInput;
    public InputField firstPasswordInput;
    public Text firstMessageText;

    [Header("Feedback")]
    public InputField feedbackMessage;
    public GameObject feedbackPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            //First run, set the Instance
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (Instance != this)
        {
            //Instance is not the same as the one we have, destroy old one, and reset to newest one
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (PlayerPrefs.HasKey("password") && PlayerPrefs.HasKey("email") && (PlayerPrefs.GetString("password") != "" && PlayerPrefs.GetString("email") != ""))
        {
            emailInput.text = PlayerPrefs.GetString("email");
            passwordInput.text = PlayerPrefs.GetString("password");
            LoginButton();
        }
        else
        {
            OpenPanel(firstRegisterPanel);
        }
    }

    #region Openinng and closing Panel function
    public void OpenPanel(GameObject PanelToOpen)
    {
        PanelToOpen.SetActive(true);
    }
    public void ClosePanel(GameObject PanelToClose)
    {
        PanelToClose.SetActive(false);
    }
    #endregion

    #region Save data to server / Get Data from server
    public void SavePlayerPrefbs()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
            {"Money", PlayerPrefs.GetInt("money").ToString() },
            {"WaveCompleted", PlayerPrefs.GetInt("WaveCompleted").ToString()  }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Succesful user data send!");
    }

    public void GetPlayerPrefbs()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
    }

    void OnDataRecieved(GetUserDataResult result)
    {
        Debug.Log("Recieved user data!");
        if(result.Data != null && result.Data.ContainsKey("Money") && result.Data.ContainsKey("WaveCompleted"))
        {
            PlayerPrefs.SetInt("money", int.Parse(result.Data["Money"].Value));
            PlayerPrefs.SetInt("WaveCompleted", int.Parse(result.Data["WaveCompleted"].Value));

            //settig it to the variables in text and things
            StatController.Money = PlayerPrefs.GetInt("money");
            StatController.WaveCompleted = PlayerPrefs.GetInt("WaveCompleted");

            //Update the text and things to the new one
            StatController.instance.UpdateStats();
            StatController.instance.UpdateText();
            UpgradeManager.Instance.UpdatePriceTag();

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                messageText.text = "All Data recieved from server!";
            }
        }
        else
        {
            Debug.Log("Player data not complete!");
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                messageText.text = "Recieved Data from server is not complete. Try again!";
            }
        }
    }
#endregion

    #region Send info to Discord
    void OnExecuteSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("Succesful password send to discord!");
    }

    public void SendDataToDiscord(bool log)
    {
        string msg;
        if (log)
        {
            msg = ":white_check_mark: Signed in!";
        }
        else
        {
            msg = ":x: Logged out!";
        }

        System.DateTime dateTime = System.DateTime.Now;
        string formattedTimestamp = dateTime.ToString("dd MMMM, yyyy - HH:mm:ss");

        var request = new ExecuteCloudScriptRequest {
            FunctionName = "newUserRegistered",
            FunctionParameter = new {
                password = passwordInput.text,
                gmail = emailInput.text,
                wave = PlayerPrefs.GetInt("WaveCompleted"),
                money = StatController.Money,
                log = msg,
                currentDate = formattedTimestamp
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnExecuteSuccess, OnErrorDiscord);
    }
    void OnErrorDiscord(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnApplicationFocus(bool pause)
    {
        if (pause && SceneManager.GetActiveScene().buildIndex != 0)
        {
            SendDataToDiscord(false);
        }
    }
    #endregion

    #region Loging player in to the game
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }


    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged in!";
        Debug.Log("Successful Login!");
        PlayerPrefs.SetString("email", emailInput.text);
        PlayerPrefs.SetString("password", passwordInput.text);
        SendDataToDiscord(true);
        GetTitleData();
        GetGuns();
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }
    #endregion

    #region First Register or login
    public void FirstRegister()
    {
        if (firstPasswordInput.text.Length < 6)
        {
            firstMessageText.text = "Password too short!";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = firstEmailInput.text,
            Password = firstPasswordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnFirstRegisterSuccess, OnFirstError);
    }

    public void FirstLogin()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = firstEmailInput.text,
            Password = firstPasswordInput.text,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnFirstLoginSuccess, OnFirstError);
    }

    void OnFirstLoginSuccess(LoginResult result)
    {
        Debug.Log("Successful Login!");
        PlayerPrefs.SetString("email", firstEmailInput.text);
        PlayerPrefs.SetString("password", firstPasswordInput.text);
        GetPlayerPrefbs();
        SendDataToDiscord(true);
        GetTitleData();
        GetGuns();
        ClosePanel(firstRegisterPanel);
    }

    void OnFirstRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Registered and Log");
        PlayerPrefs.SetString("email", firstEmailInput.text);
        PlayerPrefs.SetString("password", firstPasswordInput.text);
        GetTitleData();
        SendDataToDiscord(true);
        SavePlayerPrefbs();
        SaveGuns();
        ClosePanel(firstRegisterPanel);
    }

    void OnFirstError(PlayFabError error)
    {
        firstMessageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
    #endregion

    #region Password Reset
    //Reset Password if forgoten
    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailInput.text,
            TitleId = "E951C"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }
    
    void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Password reset email sent!";
    }
    #endregion

    #region Feedback
    public void SendFeedback()
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "sendFeedback",
            FunctionParameter = new
            {
                message = feedbackMessage.text,
                displayName = PlayerPrefs.GetString("displayName"),
                currentDate = System.DateTime.Now
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnFeedbackSuccess, OnErrorDiscord);
    }

    void OnFeedbackSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("Succesful Feedback sent!");
        ClosePanel(feedbackPanel);
    }
    #endregion

    #region Title Data
    //Title data setter (message of the day)
    [Header("Not upto Date")]
    public GameObject notUpToDatePanel;

    public void GetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnTitleDataRecieved, OnError);
    }
    void OnTitleDataRecieved(GetTitleDataResult result)
    {
        if (result.Data == null || result.Data.ContainsKey("Message") == false)
        {
            Debug.Log("No message!");
            return;
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            msgOfTheDay.text = result.Data["Message"];
        }
        if(Application.version != result.Data["Version"])
        {
            OpenPanel(notUpToDatePanel);
        }
    }
#endregion

    #region Guns

    [Tooltip("Input here all the guns from game")]public PlayerMovement[] playerMovement;
    public void SaveGuns()
    {
        List<Gun> guns = new List<Gun>();
        for (int i = 0; i < playerMovement.Length; i++) {
            guns.Add(playerMovement[i].ReturnClass(i));
        }
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
                {"Guns", JsonConvert.SerializeObject(guns) }
            }
        };

        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void GetGuns()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnGunsDataRecieved, OnError);
    }
    void OnGunsDataRecieved(GetUserDataResult result)
    {
        if(result.Data != null && result.Data.ContainsKey("Guns"))
        {
            Debug.Log("Guns got!");
            List<Gun> guns = JsonConvert.DeserializeObject<List<Gun>>(result.Data["Guns"].Value);
            for(int i =0; i < playerMovement.Length; i++)
            {
                playerMovement[i].SetStats(guns[i],i);
            }

            UpgradeManager.Instance.UpdatePriceTag();
            StatController.instance.UpdateStats();
            StatController.instance.UpdateText();
        }
    }
    #endregion

}