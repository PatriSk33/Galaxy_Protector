using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GunStats
{
    public float startingFireRate;
    public float startingDamage;
}

public class StatController : MonoBehaviour
{
    public static StatController Instance { get; private set; }

    // Player stats
    public static float Damage;
    public static float FireRate;
    public static float Health;
    public static float MaxHealth;

    // Ship selection
    public static int selected = 0;

    // Waves
    public static int Wave, WaveCompleted;
    public TMP_Text waveText;

    // Money & upgrades
    public static int Money;
    public TMP_Text moneyText;
    public static int[] FireRateLvl = new int[4];
    public static int[] DamageLvl = new int[4];

    // Enemies killed (for leaderboard, maybe)
    public static int enemiesKilled;

    // Feedback
    public static int timesPlayed;

    // Gun base stats
    [SerializeField] private float[] startingDamage, startingFireRate;

    private void Awake()
    {
        Instance = this;

        timesPlayed = PlayerPrefs.GetInt("timesPlayed", 0);
        selected = PlayerPrefs.GetInt("selected", 0);

        if (WaveCompleted <= 0)
            WaveCompleted = 1;
    }

    private void Start()
    {
        OnStart();
    }

    public void OnStart()
    {
        // Load HexValue if HexSetter exists
        if (PlayerPrefs.HasKey("HexValue") && HexSetter.Instance != null)
        {
            HexSetter.Instance.hexValue = PlayerPrefs.GetString("HexValue");
            HexSetter.Instance.SetHexColor(HexSetter.Instance.hexValue);
        }

        // Health setup
        SetMaximumHP();
        Health = MaxHealth;

        UpdateStats();

        // Finished condition
        if (WaveCompleted > 100) WaveCompleted = 100;

        Wave = WaveCompleted;

        UpdateText();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && SceneManager.GetActiveScene().buildIndex == 0)
        {
            Save();
            Debug.Log("App paused, progress saved.");
        }
    }

    public void UpdateText()
    {
        waveText.text = Wave + ".Wave";
        moneyText.text = Money.ToString();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("selected", selected);
        PlayerPrefs.SetInt("timesPlayed", timesPlayed);

        // Save upgrades
        for (int i = 0; i < FireRateLvl.Length; i++)
            PlayerPrefs.SetInt("FireRateLvl_" + i, FireRateLvl[i]);

        for (int i = 0; i < DamageLvl.Length; i++)
            PlayerPrefs.SetInt("DamageLvl_" + i, DamageLvl[i]);

        PlayerPrefs.Save();
    }

    public void Load()
    {
        for (int i = 0; i < FireRateLvl.Length; i++)
            FireRateLvl[i] = PlayerPrefs.GetInt("FireRateLvl_" + i, 0);

        for (int i = 0; i < DamageLvl.Length; i++)
            DamageLvl[i] = PlayerPrefs.GetInt("DamageLvl_" + i, 0);

        UpdateStats();
        UpdateText();
    }

    public void UpdateStats()
    {
        GunStats[] gunStats = new GunStats[LocalDataManager.Instance.GetPlayers().Length];

        for (int i = 0; i < gunStats.Length; i++)
        {
            gunStats[i] = new GunStats
            {
                startingFireRate = startingFireRate[i],
                startingDamage = startingDamage[i]
            };
        }

        float fireRate = gunStats[selected].startingFireRate - (FireRateLvl[selected] * 0.08f);
        float damage = gunStats[selected].startingDamage + (DamageLvl[selected] * 0.8f);

        FireRate = Mathf.Clamp(fireRate, 0.3f, 1.8f);
        Damage = Mathf.Clamp(damage, 2f, 18.8f);
    }

    private static readonly float[] maxHealthValues = { 25f, 50f, 100f, 150f };

    private void SetMaximumHP()
    {
        int selectedValue = Mathf.Clamp(selected, 0, maxHealthValues.Length - 1);
        MaxHealth = maxHealthValues[selectedValue];
    }
}
