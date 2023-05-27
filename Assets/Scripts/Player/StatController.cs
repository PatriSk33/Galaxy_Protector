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
    public static float Damage = 1;
    public static float FireRate = 1.5f;
    public static float Health = 10;
    public static float MaxHealth = 10;

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

    public static void CheckHealth()
    {
        if (Health <= 0)
        {
            SceneManager.LoadScene(0);
            AfterGameController.won = false;
            AfterGameController.instance.ShowPanel();
            Health = MaxHealth;
        }
    }

    private void Awake()
    {
        instance = this;

        Money = PlayerPrefs.GetInt("money", 0);
        WaveCompleted = PlayerPrefs.GetInt("WaveCompleted", 1);
        selected = PlayerPrefs.GetInt("selected", 0);
    }

    private void Start()
    {
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
        if(WaveCompleted <= 1) { Wave = 1; }
    }

    public void FinishedOff(){finishedScreen.SetActive(false);}

    private void OnApplicationQuit()
    {
        Save();
        Debug.Log("quited the app!!!!");
        if (PlayerPrefs.HasKey("password") && PlayerPrefs.HasKey("email"))
        {
            PlayfabManager.Instance.SavePlayerPrefbs();
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
    }

    public void UpdateStats()
    {
        GunStats[] gunStats = new GunStats[PlayfabManager.Instance.playerMovement.Length];

        // Set individual starting values for each gun
        for (int i = 0; i < gunStats.Length; i++)
        {
            gunStats[i] = new GunStats();
            gunStats[i].startingFireRate = startingDamage[i];
            gunStats[i].startingDamage = startingFireRate[i];
        }

        float fireRate;
        float damage;
        for (int i = 0; i < PlayfabManager.Instance.playerMovement.Length; i++)
        {
            fireRate = gunStats[i].startingFireRate - (FireRateLvl[i] * 0.08f);
            damage = gunStats[i].startingDamage + (DamageLvl[i] * 0.8f);

            FireRate = Mathf.Clamp(fireRate, 0.3f, 2);
            Damage = Mathf.Clamp(damage, 2f, 18f);
        }
    }


    private static readonly float[] maxHealthValues = { 25f, 50f, 75f, 100f};
    private void CheckMaximumHP()
    {
        int selectedValue = Mathf.Clamp(selected, 0, maxHealthValues.Length - 1);
        MaxHealth = maxHealthValues[selectedValue];
    }
}
