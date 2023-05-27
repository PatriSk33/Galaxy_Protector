using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterGameController : MonoBehaviour
{
    public static AfterGameController instance;
    public Text ifCompleted, amountAdded;
    public GameObject AfterGamePanel;
    public static bool won;
    public static int addedMoney;

    private void Awake()
    {
        instance = this;
    }

    public void Continue()
    {
        AfterGamePanel.SetActive(false);
        StatController.Money += addedMoney;
        addedMoney = 0;
        StatController.instance.UpdateText();
        StatController.instance.Save();
    }
    public void ShowPanel()
    {
        Debug.Log("Opened After Game panel");
        AfterGamePanel.SetActive(true);
        UpdateText();
    }

    public void UpdateText()
    {
        if (won) {
            ifCompleted.text = "Wave " + (StatController.Wave - 1).ToString() + " Completed"; 
        }
        else if (!won)
        {
            ifCompleted.text = "Wave " + (StatController.Wave).ToString() + " Lost";
        }
        amountAdded.text = "+ " + addedMoney.ToString();
    }

}
