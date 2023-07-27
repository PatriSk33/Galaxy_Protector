using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUIButtons : MonoBehaviour
{
    public static GameplayUIButtons Instance { get; private set; }
    public SetupSpaceship spaceshipChooser;
    float curRot = 0;

    public void Awake()
    {
        Instance = this;
        canRevive = true;
    }

    //Pausing Gameplay
    public GameObject pausePanel;

    private void OnApplicationPause(bool pause)
    {
        if (pause && SceneManager.GetActiveScene().buildIndex == 1)
        {
            Pause();
        }
    }
    public void ContinueInGame()
    {
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
    }
    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
        AfterGameController.addedMoney = 0;
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }


    // Reviving and sending you to main menu
    public GameObject RevivePanel;
    [HideInInspector] public bool canRevive;

    public void OpenRevivePanel()
    {
        RevivePanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Revive()
    {
        if (canRevive)
        {
            LevelPlayAds.Instance.ShowRewardedAd("Revive_Player");
        }
    }

    public void ContinueToMenu() // In Revive Panel
    {
        Time.timeScale = 1.0f;
        OpenAfterGame();
        RevivePanel.SetActive(false);
    }

    public void AfterAdRevive()
    {
        canRevive = false;
        RevivePanel.SetActive(false);
        StatController.Health = StatController.MaxHealth / 5;
        spaceshipChooser.ActivateSpaceship();
    }


    // After Game Panel / AD
    public void OpenAfterGame()
    {
        AfterGameController.won = false;
        AfterGameController.Instance.ShowPanel();
    }

    public void DoubleMoneyAd()
    {
        LevelPlayAds.Instance.ShowRewardedAd("Double_Money");
    }


    // Health
    public void ChechHealth()
    {
        if (StatController.Health <= 0 && canRevive)
        {
            OpenRevivePanel();
        }
        else if (StatController.Health <= 0 && !canRevive)
        {
            OpenAfterGame();
        }
    }

    private void Update()
    {
        ChechHealth();

        curRot += Time.deltaTime;
        curRot %= 360;
        RenderSettings.skybox.SetFloat("_Rotation", curRot);
    }
}
