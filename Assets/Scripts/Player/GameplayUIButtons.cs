using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUIButtons : MonoBehaviour
{
    public static GameplayUIButtons instance;
    public void Awake()
    {
        instance = this;
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
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }


    // Reviving and sending you to main menu
    public GameObject RevivePanel;
    public GameObject ButtonRevive;
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

    [Tooltip("In Revive Panel")]
    public void ContinueToMenu()
    {
        Time.timeScale = 1.0f;
        OpenAfterGame();
        ButtonRevive.SetActive(true);
        RevivePanel.SetActive(false);
    }

    public void AfterAdRevive()
    {
        canRevive = false;
        RevivePanel.SetActive(false);
        StatController.Health = 10;
        Time.timeScale = 1;
    }

    public void OpenAfterGame()
    {
        AfterGameController.won = false;
        AfterGameController.instance.ShowPanel();
    }



    public void DoubleMoneyAd()
    {
        LevelPlayAds.Instance.ShowRewardedAd("Double_Money");
    }
}
