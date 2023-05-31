using System.Collections;
using System.Collections.Generic;
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
    //instance
    public static StatController instance;

    //Player
    public static float Damage;
    public static float FireRate;
    public static float Health;
    public static float MaxHealth;

    //Ship
    public static int selected = 0;

    //Waves
    public static int Wave, WaveCompleted;
    public Text waveText;

    //Money & Upgrades
    public static int Money;
    public Text moneyText;
    public static int[] FireRateLvl = new int[4]; // fire rate lvl for each gun separately
    public static int[] DamageLvl = new int[4]; // damage lvl for each gun separately


    //Finished
    public GameObject finishedScreen;
    public bool finished;

    public PlayfabManager playfabManager;

    //Stats for Guns
    [SerializeField]private float[] startingDamage, startingFireRate;

    private void Awake()
    {
        instance = this;

        Money = PlayerPrefs.GetInt("money", 0);
        WaveCompleted = PlayerPrefs.GetInt("WaveCompleted", 1);
        selected = PlayerPrefs.GetInt("selected", 0);
    }

    private void Start()
    {
        //---------------------------------Cheats----------------------------//
        //Money = 5000;

        //Health
        CheckMaximumHP();
        Health = MaxHealth;

        UpdateStats();
        UpdateText();

        //Finished
        if (WaveCompleted > 100) { WaveCompleted = 100; }
        if (WaveCompleted == 100 && finished)
        {
            waveText.text = "Finished";
            finishedScreen.SetActive(true);
        }
        Wave = WaveCompleted; 
        UpdateText(); 
    }

    public void FinishedOff(){ finishedScreen.SetActive(false); }

    private void OnApplicationPause(bool pause)
    {
        if (pause && SceneManager.GetActiveScene().buildIndex == 0)
        {
            Save();
            Debug.Log("quited the app!!!!");
            if (PlayerPrefs.HasKey("password") && PlayerPrefs.HasKey("email"))
            {
                PlayfabManager.Instance.SavePlayerPrefbs();
                PlayfabManager.Instance.SaveGuns();
            }
        }
    }

    public void UpdateText()
    {
        waveText.text = Wave + ".Wave";
        moneyText.text = Money.ToString();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("WaveCompleted", WaveCompleted);
        PlayerPrefs.SetInt("money", Money);
        PlayerPrefs.SetInt("selected", selected);
        if (PlayerPrefs.HasKey("password") && PlayerPrefs.HasKey("email"))
        {
            PlayfabManager.Instance.SavePlayerPrefbs();
        }
    }

    public void UpdateStats()
    {
        GunStats[] gunStats = new GunStats[PlayfabManager.Instance.playerMovement.Length];

        // Set individual starting values for each gun
        for (int i = 0; i < gunStats.Length; i++)
        {
            gunStats[i] = new GunStats();
            gunStats[i].startingFireRate = startingFireRate[i];
            gunStats[i].startingDamage = startingDamage[i];
        }

        float fireRate;
        float damage;

        fireRate = gunStats[selected].startingFireRate - (FireRateLvl[selected] * 0.08f);
        damage = gunStats[selected].startingDamage + (DamageLvl[selected] * 0.8f);

        FireRate = Mathf.Clamp(fireRate, 0.3f, 1.8f);
        Damage = Mathf.Clamp(damage, 2f, 18f);
    }


    private static readonly float[] maxHealthValues = { 20f, 60f, 100f, 150f};
    private void CheckMaximumHP()
    {
        int selectedValue = Mathf.Clamp(selected, 0, maxHealthValues.Length - 1);
        MaxHealth = maxHealthValues[selectedValue];
    }
}
