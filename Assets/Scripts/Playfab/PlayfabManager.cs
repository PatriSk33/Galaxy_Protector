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
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("password") && PlayerPrefs.HasKey("email") && (PlayerPrefs.GetString("password") != "" && PlayerPrefs.GetString("email") != ""))
        {
            emailInput.text = PlayerPrefs.GetString("email");
            passwordInput.text = PlayerPrefs.GetString("password");
            LoginButton();
        }
        else {
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
            /*{"Dual",  PlayerPrefs.GetInt("Dual").ToString() },
            {"Triple", PlayerPrefs.GetInt("Triple").ToString() },
            {"Cannon", PlayerPrefs.GetInt("Cannon").ToString() }*/
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    public void GetPlayerPrefbs()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            messageText.text = "Succesful user data send!";
        }
        Debug.Log("Succesful user data send!");
    }

    void OnDataRecieved(GetUserDataResult result)
    {
        Debug.Log("Recieved user data!");
        if(result.Data != null && result.Data.ContainsKey("Money") && result.Data.ContainsKey("WaveCompleted") /*&& result.Data.ContainsKey("DmgLvl") && result.Data.ContainsKey("FireRateLvl") && result.Data.ContainsKey("Dual") && result.Data.ContainsKey("Triple") && result.Data.ContainsKey("Cannon")*/)
        {
            PlayerPrefs.SetInt("money", int.Parse(result.Data["Money"].Value));
            PlayerPrefs.SetInt("WaveCompleted", int.Parse(result.Data["WaveCompleted"].Value));
            /*PlayerPrefs.SetInt("Dual", int.Parse(result.Data["Dual"].Value));
            PlayerPrefs.SetInt("Triple", int.Parse(result.Data["Triple"].Value));
            PlayerPrefs.SetInt("Cannon", int.Parse(result.Data["Cannon"].Value));*/

            //settig it to the variables in text and things
            StatController.Money = PlayerPrefs.GetInt("money");
            StatController.WaveCompleted = PlayerPrefs.GetInt("WaveCompleted");

            //Update the text and things to the new one
            StatController.instance.UpdateStats();
            StatController.instance.UpdateText();
            UpgradeManager.Instance.UpdatePriceTag();
            //ShopController.instance.GetIfBuyed();

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

    public void SendDataToDiscord()
    {
        var request = new ExecuteCloudScriptRequest {
            FunctionName = "newUserRegistered",
            FunctionParameter = new {
                password = passwordInput.text,
                gmail = emailInput.text,
                wave = PlayerPrefs.GetInt("WaveCompleted"),
                money = StatController.Money,
                currentDate = System.DateTime.Now
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnExecuteSuccess, OnErrorDiscord);
    }
    void OnErrorDiscord(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    #endregion

    #region Loging and registring player in to the game
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
        SendDataToDiscord();
        GetPlayerPrefbs();
        GetTitleData();
        GetGuns();
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and Loged in";
        Debug.Log("Registered and Log");
        PlayerPrefs.SetString("email", emailInput.text);
        PlayerPrefs.SetString("password", passwordInput.text);
        SendDataToDiscord();
        SavePlayerPrefbs();
        GetTitleData();
    }

    public void RegisterButton()
    {
        if (passwordInput.text.Length < 6)
        {
            messageText.text = "Password too short!";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
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
        SendDataToDiscord();
        GetPlayerPrefbs();
        GetTitleData();
        GetGuns();
        ClosePanel(firstRegisterPanel);
    }

    void OnFirstRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Registered and Log");
        PlayerPrefs.SetString("email", firstEmailInput.text);
        PlayerPrefs.SetString("password", firstPasswordInput.text);
        SendDataToDiscord();
        GetTitleData();
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

    #region Title Data
    //Title data setter (message of the day)
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