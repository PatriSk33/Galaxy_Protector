using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    public string email;
    public string password;
    public string displayName;
    public PlayerStats stats;
    public List<Gun> guns;
}

[System.Serializable]
public class PlayerStats
{
    public int money;
    public int waveCompleted;
    public int enemiesKilled;
    public int hexSetterBought;
}

public static class SaveSystem
{
    private static string filePath => Path.Combine(Application.persistentDataPath, "playerData.json");

    public static void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public static PlayerData Load()
    {
        if (!File.Exists(filePath)) return null;
        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<PlayerData>(json);
    }

    public static bool HasData() => File.Exists(filePath);
}

public class LocalDataManager : MonoBehaviour
{
    public static LocalDataManager Instance;

    [Header("Register Panel")]
    [SerializeField] private GameObject firstRegisterPanel;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_Text errorText;

    [Header("Player Guns")]
    [SerializeField] private Player[] playerMovementArray;

    private string dataPath;
    private PlayerData playerData = new PlayerData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        registerButton.onClick.AddListener(Register);
        loginButton.onClick.AddListener(Login);

        dataPath = Application.persistentDataPath + "/playerData.json";
        LoadData();
    }

    private void Start()
    {
        /*if (!string.IsNullOrEmpty(playerData.email))
        {
            Login();
        }
        else
        {
            OpenPanel(firstRegisterPanel);
        }*/
    }

    public void Register()
    {
        if (passwordInput.text.Length < 6 || nameInput.text.Length < 3 || nameInput.text.Length >= 18)
        {
            errorText.text = "Invalid name or password.";
            return;
        }

        playerData.email = emailInput.text;
        playerData.password = passwordInput.text;
        playerData.displayName = nameInput.text;

        SaveData();
        ApplyLoadedData();
        ClosePanel(firstRegisterPanel);
    }

    public void Login()
    {
        if (emailInput.text == playerData.email && passwordInput.text == playerData.password)
        {
            errorText.text = "Logged in!";
            ApplyLoadedData();
        }
        else
        {
            errorText.text = "Login failed.";
        }
    }

    public void SaveData()
    {
        playerData.stats.money = StatController.Money;
        playerData.stats.waveCompleted = StatController.WaveCompleted;
        playerData.stats.enemiesKilled = StatController.enemiesKilled;
        playerData.stats.hexSetterBought = PlayerPrefs.GetInt("hexSetterBought");

        playerData.guns.Clear();
        foreach (var player in playerMovementArray)
        {
            playerData.guns.Add(player.ReturnClass(Array.IndexOf(playerMovementArray, player)));
        }

        File.WriteAllText(dataPath, JsonUtility.ToJson(playerData));
    }

    public void LoadData()
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
    }

    public void ApplyLoadedData()
    {
        StatController.Money = playerData.stats.money;
        StatController.WaveCompleted = playerData.stats.waveCompleted;
        StatController.enemiesKilled = playerData.stats.enemiesKilled;
        StatController.Wave = playerData.stats.waveCompleted;
        PlayerPrefs.SetInt("hexSetterBought", playerData.stats.hexSetterBought);

        for (int i = 0; i < playerMovementArray.Length; i++)
        {
            playerMovementArray[i].SetStats(playerData.guns[i], i);
        }

        UpgradeManager.Instance.UpdatePriceTag();
        StatController.Instance.UpdateStats();
        StatController.Instance.UpdateText();
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public Player[] GetPlayers()
    {
        return playerMovementArray;
    }
}