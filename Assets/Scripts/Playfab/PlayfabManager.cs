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
    public InputField NameInput;

    [Header("Text")]
    public Text messageText;
    public Text msgOfTheDay;

    [Header("First Register")]
    public GameObject firstRegisterPanel;
    public InputField firstEmailInput;
    public InputField firstPasswordInput;
    public InputField firstNameInput;
    public Text firstMessageText;

    [Header("Feedback")]
    public InputField feedbackMessage;
    public GameObject feedbackPanel;

    [Header("Network")]
    public GameObject noWifiPanel;
    public static bool clientConnected; // Wifi ON

    [Header("Wheel")]
    public SpinningWheel spinningWheel;

    [Header("No Name")]
    public GameObject DisplayNamePanel;
    public InputField NoNameInput;
    public Text NoMessageText;

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

        clientConnected = isConnected();
        if (clientConnected)
        {
            OnOpen();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            Time.timeScale = 0f;
            OpenPanel(noWifiPanel);
        }
    }

    private void Update()
    {
        clientConnected = isConnected();
        if (!clientConnected && SceneManager.GetActiveScene().buildIndex == 0)
        {
            Time.timeScale = 0f;
            OpenPanel(noWifiPanel);
        }
    }

    public void OnOpen()
    {
        if (PlayerPrefs.HasKey("password") && PlayerPrefs.HasKey("email") && (PlayerPrefs.GetString("password") != "" && PlayerPrefs.GetString("email") != ""))
        {
            emailInput.text = PlayerPrefs.GetString("email");
            passwordInput.text = PlayerPrefs.GetString("password");
            NameInput.text = PlayerPrefs.GetString("displayName");
            LoginButton();
        }
        else
        {
            OpenPanel(firstRegisterPanel);
        }
    }

    #region Network
    
    bool isConnected()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return false;
        else return true;
    }

    public IEnumerator RetryConnection()
    {
        clientConnected = isConnected();
        if(clientConnected)
        {
            Time.timeScale = 1f;
            ClosePanel(noWifiPanel);
            OnOpen();
            yield return new WaitForSeconds(2);
            StatController.Instance.OnStart();
        }
    }

    public void RetryConnectionCo()
    {
        StartCoroutine(RetryConnection());
    }
    #endregion

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
            {"Money", StatController.Money.ToString() },
            {"WaveCompleted", StatController.WaveCompleted.ToString()  },
            {"EnemiesKilled", StatController.enemiesKilled.ToString() },
            {"hexSetterBought", PlayerPrefs.GetInt("hexSetterBought").ToString() }
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
        if(result.Data != null && result.Data.ContainsKey("Money") && result.Data.ContainsKey("WaveCompleted") && result.Data.ContainsKey("EnemiesKilled") && result.Data.ContainsKey("hexSetterBought"))
        {
            //settig it to the variables in text and things
            StatController.Money = int.Parse(result.Data["Money"].Value);
            StatController.WaveCompleted = int.Parse(result.Data["WaveCompleted"].Value);
            StatController.enemiesKilled = int.Parse(result.Data["EnemiesKilled"].Value);
            PlayerPrefs.SetInt("hexSetterBought", int.Parse(result.Data["hexSetterBought"].Value));

            StatController.Wave = StatController.WaveCompleted;

            //Update the text and things to the new one
            StatController.Instance.UpdateStats();
            StatController.Instance.UpdateText();
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
                displayName = PlayerPrefs.GetString("displayName"),
                gmail = PlayerPrefs.GetString("email"),
                password = PlayerPrefs.GetString("password"),
                wave = StatController.WaveCompleted,
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
        loggedInPlayfabID = result.PlayFabId;
        messageText.text = "Logged in!";
        Debug.Log("Successful Login!");
        PlayerPrefs.SetString("email", emailInput.text);
        PlayerPrefs.SetString("password", passwordInput.text);
        PlayerPrefs.SetString("displayName", NameInput.text);
        GetPlayerPrefbs();
        SendDataToDiscord(true);
        GetTitleData();
        GetGuns();
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        if (name == null)
        {
            OpenPanel(DisplayNamePanel);
        }
    }

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
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

        if (firstNameInput.text.Length < 3)
        {
            firstMessageText.text = "Display Name too short!";
            return;
        }
        if (firstNameInput.text.Length >= 18)
        {
            firstMessageText.text = "Display Name too long!";
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = firstNameInput.text,
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
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnFirstLoginSuccess, OnFirstError);
    }

    void OnFirstLoginSuccess(LoginResult result)
    {
        loggedInPlayfabID = result.PlayFabId;
        Debug.Log("Successful Login!");
        PlayerPrefs.SetString("email", firstEmailInput.text);
        PlayerPrefs.SetString("password", firstPasswordInput.text);
        PlayerPrefs.SetString("displayName", firstNameInput.text);
        GetPlayerPrefbs();
        SendDataToDiscord(true);
        GetTitleData();
        GetGuns();
        ClosePanel(firstRegisterPanel);
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        if (name == null)
        {
            OpenPanel(DisplayNamePanel);
        }
    }

    void OnFirstRegisterSuccess(RegisterPlayFabUserResult result)
    {
        loggedInPlayfabID = result.PlayFabId;
        Debug.Log("Registered and Log");
        PlayerPrefs.SetString("email", firstEmailInput.text);
        PlayerPrefs.SetString("password", firstPasswordInput.text);
        PlayerPrefs.SetString("displayName", firstNameInput.text);
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

    #region Leaderboard
    [Header("Leaderboard")]
    public GameObject rowPrefab;
    public Transform rowsParent;
    string loggedInPlayfabID;

    public void SendLeaderboard()
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "EnemiesKilled",
                    Value = StatController.enemiesKilled
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Succesful leaderboard data sent");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest()
        {
            StatisticName = "EnemiesKilled",
            StartPosition = 0,
            MaxResultsCount = 25
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    public void GetLeaderboardAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "EnemiesKilled",
            MaxResultsCount = 25
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnError);
    }

    void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
    {
        int rows = 0;
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            if (item.PlayFabId == loggedInPlayfabID)
            {
                texts[0].color = Color.cyan;
                texts[1].color = Color.cyan;
                texts[2].color = Color.cyan;
                texts[0].fontSize = 80;
                texts[1].fontSize = 80;
                texts[2].fontSize = 80;
            }

            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
            rows++;
        }
        for (int i = rows; i < 26; i++)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = "";
            texts[1].text = "";
            texts[2].text = "";
        }
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        int rows = 0;
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            if (item.PlayFabId == loggedInPlayfabID)
            {
                texts[0].color = Color.cyan;
                texts[1].color = Color.cyan;
                texts[2].color = Color.cyan;
                texts[0].fontSize = 80;
                texts[1].fontSize = 80;
                texts[2].fontSize = 80;
            }

            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
            rows++;
        }
        for (int i = rows; i < 26; i++)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = "";
            texts[1].text = "";
            texts[2].text = "";
        }
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

    #region Display Name setter
    public void UpdateDisplayName()
    {
        if (NameInput.text == "")
        {
            return;
        }
        if (NameInput.text.Length < 3)
        {
            return;
        }
        if (NameInput.text.Length >= 18)
        {
            return;
        }
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = NameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    public void SetDisplayName()
    {
        if (NoNameInput.text.Length < 3)
        {
            NoMessageText.text = "Display Name too short!";
            return;
        }
        if (NoNameInput.text.Length >= 18)
        {
            NoMessageText.text = "Display Name too long!";
            return;
        }
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = NoNameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameSent, OnError);
    }

    void OnDisplayNameSent(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Sent display name!");
        ClosePanel(DisplayNamePanel);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
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

        AfterGameController.multiplier = float.Parse(result.Data["Multiplier"]);
    }
#endregion

    #region Guns

    [Tooltip("Input here all the guns from game")]public Player[] playerMovement;
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
            for(int i = 0; i < playerMovement.Length; i++)
            {
                playerMovement[i].SetStats(guns[i],i);
            }

            if (!ShopController.buyed[StatController.selected] || StatController.selected * 25 > StatController.WaveCompleted)
            {
                StatController.selected = 0;
                PlayerPrefs.SetInt("selected", 0);
            }

            UpgradeManager.Instance.UpdatePriceTag();
            StatController.Instance.UpdateStats();
            StatController.Instance.UpdateText();
        }
    }
    #endregion

}