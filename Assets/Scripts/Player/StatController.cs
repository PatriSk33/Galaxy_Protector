using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public static int selected;

    //Waves
    public static int Wave;
    public Text waveText;

    //Money & Upgrades
    public static int Money;
    public Text moneyText;
    public static int FRUpgrade; // fire rate upgrade
    public static int DUpgrade; // damage upgrade


    //Finished
    public GameObject finishedScreen;

    //Bonus
    public int bonus;

    public PlayfabManager playfabManager;

    //Logging
    public Toggle toggle;

    public static void CheckHealth()
    {
        if (Health <= 0)
        {
            AfterGameController.won = false;
            AfterGameController.afterGame = true;
            SceneManager.LoadScene(0);
            Health = MaxHealth;
        }
    }

    private void Awake()
    {
        instance = this;

        //Health
        CheckMaximumHP();
        Health = MaxHealth;

        if (PlayerPrefs.HasKey("money"))
        {
            Money = PlayerPrefs.GetInt("money");
        }
        if (PlayerPrefs.HasKey("wave"))
        {
            Wave = PlayerPrefs.GetInt("wave");
        }

        selected = PlayerPrefs.GetInt("selected");
        UpdateStats();
        UpdateText();

        //Finished
        if (Wave >= 12) { Wave = 11; }
        if (Wave == 11)
        {
            waveText.text = "Finished";
            finishedScreen.SetActive(true);
        }
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
        PlayerPrefs.SetInt("wave", Wave);
        PlayerPrefs.SetInt("money", Money);
        PlayerPrefs.SetInt("selected", selected);
    }

    public void UpdateStats()
    {
        switch (FRUpgrade)
        {
            case 0:
                FireRate = 1.5f;
                break;
            case 1:
                FireRate = 1.4f;
                break;
            case 2:
                FireRate = 1.3f;
                break;
            case 3:
                FireRate = 1.2f;
                break;
            case 4:
                FireRate = 1.1f;
                break;
            case 5:
                FireRate = 1;
                break;
            case 6:
                FireRate = 0.9f;
                break;
            case 7:
                FireRate = 0.8f;
                break;
            case 8:
                FireRate = 0.7f;
                break;
            case 9:
                FireRate = 0.6f;
                break;
            case 10:
                FireRate = 0.5f;
                break;
            case 11:
                FireRate = 0.5f;
                break;
        }

        switch (DUpgrade)
        {
            case 0:
                Damage = 1;
                break;
            case 1:
                Damage = 1.5f;
                break;
            case 2:
                Damage = 2f;
                break;
            case 3:
                Damage = 3f;
                break;
            case 4:
                Damage = 4f;
                break;
            case 5:
                Damage = 4.5f;
                break;
            case 6:
                Damage = 5f;
                break;
            case 7:
                Damage = 6f;
                break;
            case 8:
                Damage = 6.5f;
                break;
            case 9:
                Damage = 7f;
                break;
            case 10:
                Damage = 8f;
                break;
            case 11:
                Damage = 8.1f;
                break;
        }
    }

    private void CheckMaximumHP()
    {
        switch(selected) 
        { 
            case 0:
                MaxHealth = 10;
                break;
            case 1:
                MaxHealth = 14;
                break;
            case 2:
                MaxHealth = 18;
                break;
            case 3:
                MaxHealth = 20;
                break;
        }
    }
}
