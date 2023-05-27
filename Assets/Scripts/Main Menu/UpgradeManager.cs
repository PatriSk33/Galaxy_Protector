using PlayFab.EconomyModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    public Text DmgCostText, FireRateCostText, DmgLvlText, FireRateLvlText;
    private int DmgPrice, FireRatePrice;

    //Disable after max lvl
    public GameObject dmgDisable, fireRateDisable;

    public AudioSource notEnoughMoneySound;
    public AudioSource powerup;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        /*if (PlayerPrefs.HasKey("DmgLvl"))
        {
            DmgLvl = PlayerPrefs.GetInt("DmgLvl");
            StatController.DUpgrade[StatController.selected] = DmgLvl;
        }
        if (PlayerPrefs.HasKey("FireRateLvl"))
        {
            FireRateLvl = PlayerPrefs.GetInt("FireRateLvl");
            StatController.FRUpgrade[StatController.selected] = FireRateLvl;
        }*/

        UpdatePriceTag();
    }

    public void UpgradeFireRate()
    {
        if (StatController.FireRateLvl[StatController.selected] < 5)
        {
            if (StatController.Money >= FireRatePrice)
            {
                StatController.Money -= FireRatePrice;
                StatController.FireRateLvl[StatController.selected]++;

                UpdatePriceTag();
                
                StatController.instance.UpdateText();
                StatController.instance.UpdateStats();
                StatController.instance.Save();
                PlayfabManager.Instance.SaveGuns();

                powerup.Play();

                //PlayerPrefs.SetInt("FireRateLvl", FireRateLvl);
            }
            else
            {
                Debug.Log("U don't have enough Money");
                notEnoughMoneySound.Play();
            }
        }
        else 
        {
            if (StatController.Money >= FireRatePrice && StatController.FireRateLvl[StatController.selected] == 5)
            {
                StatController.Money -= FireRatePrice;
                StatController.FireRateLvl[StatController.selected]++;
                
                fireRateDisable.SetActive(false);
                
                StatController.instance.UpdateStats();
                StatController.instance.UpdateText();
                StatController.instance.Save();
                PlayfabManager.Instance.SaveGuns();

                powerup.Play();
                //PlayerPrefs.SetInt("FireRateLvl", FireRateLvl);
            }
            else
            {
                Debug.Log("U don't have enough Money");
                notEnoughMoneySound.Play();
            }
        }
    }
    public void UpgradeDamage()
    {
        if (StatController.DamageLvl[StatController.selected] < 5)
        {
            if (StatController.Money >= DmgPrice)
            {
                StatController.Money -= DmgPrice;
                StatController.DamageLvl[StatController.selected]++;

                UpdatePriceTag();
                
                StatController.instance.UpdateStats();
                StatController.instance.UpdateText();
                StatController.instance.Save();
                PlayfabManager.Instance.SaveGuns();

                powerup.Play();
                
                //PlayerPrefs.SetInt("DmgLvl", DmgLvl);
            }
            else
            {
                Debug.Log("U don't have enough Money");
                notEnoughMoneySound.Play();
            }
        }
        else
        {
            if (StatController.Money >= DmgPrice && StatController.DamageLvl[StatController.selected] == 5)
            {
                StatController.Money -= DmgPrice;
                StatController.DamageLvl[StatController.selected]++;

                dmgDisable.SetActive(false);
                
                StatController.instance.UpdateStats();
                StatController.instance.UpdateText();
                StatController.instance.Save();
                PlayfabManager.Instance.SaveGuns();

                powerup.Play();

                //PlayerPrefs.SetInt("DmgLvl", DmgLvl);
            }
            else
            {
                Debug.Log("U don't have enough Money");
                notEnoughMoneySound.Play();
            }
        }
    }

    public void UpdatePriceTag()
    {
        int[] prices = { 100, 250, 500, 750, 1000 };

        DmgPrice = prices[Mathf.Min(StatController.DamageLvl[StatController.selected], prices.Length - 1)];
        FireRatePrice = prices[Mathf.Min(StatController.FireRateLvl[StatController.selected], prices.Length - 1)];

        DmgCostText.text = (StatController.DamageLvl[StatController.selected] < 6) ? DmgPrice.ToString() : "";
        DmgLvlText.text = (StatController.DamageLvl[StatController.selected] < 6) ? ("LVL. " + StatController.DamageLvl[StatController.selected]) : "LVL. MAX";

        FireRateCostText.text = (StatController.FireRateLvl[StatController.selected] < 6) ? FireRatePrice.ToString() : "";
        FireRateLvlText.text = (StatController.FireRateLvl[StatController.selected] < 6) ? ("LVL. " + StatController.FireRateLvl[StatController.selected]) : "LVL. MAX";
    }
}
