using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfterGameController : MonoBehaviour
{
    public static AfterGameController instance;

    public Text ifCompleted, amountAdded;
    public GameObject AfterGamePanel;
    public Button AdButton;
    public GameObject ultimateBoss;
    public Text ultimateText;

    public static bool won;
    public static float multiplier = 1;
    public static int addedMoney;

    private void Awake()
    {
        instance = this;
    }

    public void Continue()
    {
        if (StatController.Wave == 100 && EnemySpawner.Instance.numEnemiesSpawned == EnemySpawner.Instance.maxEnemies) 
        {
            GameObject enemy = Instantiate(ultimateBoss,new Vector3(0, 0, 48), Quaternion.identity);
            EnemySpawner.Instance.numEnemiesSpawned++;
            EnemySpawner.Instance.enemiesOnField.Add(enemy);

            StartCoroutine(UltimateText());
            
            AfterGamePanel.SetActive(false);
            return;
        }

        AfterGamePanel.SetActive(false);

        StatController.Money += Mathf.RoundToInt(addedMoney * multiplier);
        addedMoney = 0;

        StatController.timesPlayed++;
        StatController.Wave++;
        StatController.instance.Save();
        Time.timeScale = 1;
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    public void ShowPanel()
    {
        Debug.Log("Opened After Game panel");
        AfterGamePanel.SetActive(true);
        UpdateText();
        Time.timeScale = 0;
    }

    public void UpdateText()
    {
        if (won) {
            ifCompleted.text = "Wave " + (StatController.Wave).ToString() + " Completed"; 
        }
        else if (!won)
        {
            ifCompleted.text = "Wave " + (StatController.Wave).ToString() + " Lost";
        }
        amountAdded.text = "+ " + (Mathf.RoundToInt(addedMoney * multiplier)).ToString();
    }

    IEnumerator UltimateText()
    {
        ultimateText.gameObject.SetActive(true);
        ultimateText.text = "You though this was the END?!";
        yield return new WaitForSeconds(0.5f);
        ultimateText.text = "XD";
        yield return new WaitForSeconds(0.5f);
        ultimateText.text = "No, no!";
        yield return new WaitForSeconds(0.5f);
        ultimateText.text = "This is the Final Boss!";
        yield return new WaitForSeconds(0.5f);
        ultimateText.text = "Good Luck!";
        Time.timeScale = 1;
        ultimateText.gameObject.SetActive(false);
    }
}
