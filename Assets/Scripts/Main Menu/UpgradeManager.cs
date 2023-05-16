using PlayFab.EconomyModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public int DmgLvl, FireRateLvl;
    public Text DmgCostText, FireRateCostText, DmgLvlText, FireRateLvlText;
    private int DmgPrice, FireRatePrice;

    //Disable after max lvl
    public GameObject dmgDisable, fireRateDisable;

    public AudioSource notEnoughMoneySound;
    public AudioSource powerup;

    private void Start()
    {
        if (PlayerPrefs.HasKey("DmgLvl"))
        {
            DmgLvl = PlayerPrefs.GetInt("DmgLvl");
            StatController.DUpgrade = DmgLvl;
        }
        if (PlayerPrefs.HasKey("FireRateLvl"))
        {
            FireRateLvl = PlayerPrefs.GetInt("FireRateLvl");
            StatController.FRUpgrade = FireRateLvl;
        }

        UpdatePriceTag();
    }

    public void UpgradeFireRate()
    {
        if (FireRateLvl < 10)
        {
            if (StatController.Money >= FireRatePrice)
            {
                StatController.Money -= FireRatePrice;
                FireRateLvl++;
                StatController.FRUpgrade = FireRateLvl;
                StatController.instance.UpdateStats();
                UpdatePriceTag();
                FireRateCostText.text = FireRatePrice.ToString();
                PlayerPrefs.SetInt("FireRateLvl", FireRateLvl);
                StatController.instance.UpdateText();
                StatController.instance.Save();
                powerup.Play();
            }
            else
            {
                Debug.Log("U don't have enough Money");
                notEnoughMoneySound.Play();
            }
        }
        else 
        {
            if (StatController.Money >= FireRatePrice)
            {
                StatController.Money -= FireRatePrice;
                FireRateCostText.text = "";
                FireRateLvlText.text = "LVL. MAX";
                fireRateDisable.SetActive(false);
                if (FireRateLvl == 10) { FireRateLvl++; }
                StatController.FRUpgrade = FireRateLvl;
                StatController.instance.UpdateStats();
                PlayerPrefs.SetInt("FireRateLvl", FireRateLvl);
                StatController.instance.Save();
            }
        }
    }
    public void UpgradeDamage()
    {
        if (DmgLvl < 10)
        {
            if (StatController.Money >= DmgPrice)
            {
                StatController.Money -= DmgPrice;
                DmgLvl++;
                StatController.DUpgrade = DmgLvl;
                StatController.instance.UpdateStats();
                UpdatePriceTag();
                DmgCostText.text = DmgPrice.ToString();
                PlayerPrefs.SetInt("DmgLvl", DmgLvl);
                StatController.instance.UpdateText();
                StatController.instance.Save();
                powerup.Play();
            }
            else
            {
                Debug.Log("U don't have enough Money");
                notEnoughMoneySound.Play();
            }
        }
        else
        {
            if (StatController.Money >= DmgPrice)
            {
                StatController.Money -= DmgPrice;
                DmgCostText.text = "";
                DmgLvlText.text = "LVL. MAX";
                dmgDisable.SetActive(false);
                if (DmgLvl == 10) { DmgLvl++; }
                StatController.DUpgrade = DmgLvl;
                StatController.instance.UpdateStats();
                PlayerPrefs.SetInt("DmgLvl", DmgLvl);
                StatController.instance.Save();
            }
        }
    }

    public void UpdatePriceTag()
    {
        switch (FireRateLvl)
        {
            case 0:
                FireRatePrice = 100;
                break;
            case 1:
                FireRatePrice = 250;
                break;
            case 2:
                FireRatePrice = 500;
                break;
            case 3:
                FireRatePrice = 750;
                break;
            case 4:
                FireRatePrice = 1000;
                break;
            case 5:
                FireRatePrice = 1250;
                break;
            case 6:
                FireRatePrice = 1500;
                break;
            case 7:
                FireRatePrice = 1750;
                break;
            case 8:
                FireRatePrice = 2000;
                break;
            case 9:
                FireRatePrice = 2250;
                break;
            case 10:
                FireRatePrice = 2500;
                break;
        }
        switch (DmgLvl)
        {
            case 0:
                DmgPrice = 100;
                break;
            case 1:
                DmgPrice = 200;
                break;
            case 2:
                DmgPrice = 300;
                break;
            case 3:
                DmgPrice = 450;
                break;
            case 4:
                DmgPrice = 600;
                break;
            case 5:
                DmgPrice = 800;
                break;
            case 6:
                DmgPrice = 1000;
                break;
            case 7:
                DmgPrice = 1200;
                break;
            case 8:
                DmgPrice = 1500;
                break;
            case 9:
                DmgPrice = 1800;
                break;
            case 10:
                DmgPrice = 2000;
                break;
        }
        if (DmgLvl < 11)
        {
            DmgCostText.text = DmgPrice.ToString();
            DmgLvlText.text = "LVL. " + DmgLvl.ToString();
        }
        else
        {
            DmgCostText.text = "";
            DmgLvlText.text = "LVL. MAX";
        }

        if (FireRateLvl < 11)
        {
            FireRateLvlText.text = "LVL. " + FireRateLvl.ToString();
            FireRateCostText.text = FireRatePrice.ToString();
        }
        else
        {
            FireRateCostText.text = "";
            FireRateLvlText.text = "LVL. MAX";
        }
    }
}
