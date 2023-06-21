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
        UpdatePriceTag();
    }

    public void UpgradeFireRate()
    {
        if (PlayfabManager.clientConnected)
        {
            if (StatController.FireRateLvl[StatController.selected] < 4)
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
                }
                else
                {
                    Debug.Log("U don't have enough Money");
                    notEnoughMoneySound.Play();
                }
            }
            else if (StatController.FireRateLvl[StatController.selected] == 4)
            {
                if (StatController.Money >= FireRatePrice)
                {
                    StatController.Money -= FireRatePrice;
                    StatController.FireRateLvl[StatController.selected]++;

                    UpdatePriceTag();

                    StatController.instance.UpdateStats();
                    StatController.instance.UpdateText();
                    StatController.instance.Save();
                    PlayfabManager.Instance.SaveGuns();

                    powerup.Play();
                }
                else
                {
                    Debug.Log("U don't have enough Money");
                    notEnoughMoneySound.Play();
                }
            }
        }
    }
    public void UpgradeDamage()
    {
        if (PlayfabManager.clientConnected)
        {
            if (StatController.DamageLvl[StatController.selected] < 4)
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
                }
                else
                {
                    Debug.Log("U don't have enough Money");
                    notEnoughMoneySound.Play();
                }
            }
            else if (StatController.DamageLvl[StatController.selected] == 4)
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
                }
                else
                {
                    Debug.Log("U don't have enough Money");
                    notEnoughMoneySound.Play();
                }
            }
        }
    }

    public void UpdatePriceTag()
    {
        // Define the price arrays for each spaceship
        int[][] spaceshipPrices =
        {
        new int[] { 100, 250, 500, 750, 1000 },  // Spaceship 1 prices
        new int[] { 250, 500, 750, 1000, 1250 },  // Spaceship 2 prices
        new int[] { 500, 750, 1000, 1250, 1500 },   // Spaceship 3 prices
        new int[] { 750, 1000, 1250, 1500, 1750 }    // Spaceship 4 prices
        };

        int spaceshipIndex = StatController.selected; // Index of the selected spaceship
        int[] prices = spaceshipPrices[spaceshipIndex]; // Get the price array for the selected spaceship

        DmgPrice = prices[Mathf.Min(StatController.DamageLvl[spaceshipIndex], prices.Length - 1)];
        FireRatePrice = prices[Mathf.Min(StatController.FireRateLvl[spaceshipIndex], prices.Length - 1)];

        DmgCostText.text = (StatController.DamageLvl[spaceshipIndex] < 5 ) ? DmgPrice.ToString() : "";
        DmgLvlText.text = (StatController.DamageLvl[spaceshipIndex] < 5 ) ? "LVL. " + StatController.DamageLvl[spaceshipIndex] : "LVL. MAX";

        FireRateCostText.text = (StatController.FireRateLvl[spaceshipIndex] < 5 ) ? FireRatePrice.ToString() : "";
        FireRateLvlText.text = (StatController.FireRateLvl[spaceshipIndex] < 5 ) ? "LVL. " + StatController.FireRateLvl[spaceshipIndex] : "LVL. MAX";

        if (StatController.DamageLvl[StatController.selected] == 5)
        {
            dmgDisable.SetActive(false);
        }
        if (StatController.FireRateLvl[StatController.selected] == 5)
        {
            fireRateDisable.SetActive(false);
        }
    }
}
