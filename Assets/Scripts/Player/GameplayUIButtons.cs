using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUIButtons : MonoBehaviour
{
    public GameObject pausePanel;
    public void Continue()
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
}
