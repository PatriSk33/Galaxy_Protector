using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [SerializeField] private TMP_Text DmgCostText, FireRateCostText, DmgLvlText, FireRateLvlText;
    private int DmgPrice, FireRatePrice;

    // Disable UI buttons after max level reached
    [SerializeField] private GameObject dmgDisable, fireRateDisable;

    [SerializeField] private AudioSource notEnoughMoneySound;
    [SerializeField] private AudioSource powerup;

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
        int selected = StatController.selected;
        int currentLevel = StatController.FireRateLvl[selected];

        if (currentLevel < 5)
        {
            if (StatController.Money >= FireRatePrice)
            {
                StatController.Money -= FireRatePrice;
                StatController.FireRateLvl[selected]++;
                OnUpgradeSuccess();
            }
            else
            {
                Debug.Log("You don't have enough money for Fire Rate upgrade.");
                notEnoughMoneySound.Play();
            }
        }
        else
        {
            Debug.Log("Fire Rate is already at max level.");
        }
    }

    public void UpgradeDamage()
    {
        int selected = StatController.selected;
        int currentLevel = StatController.DamageLvl[selected];

        if (currentLevel < 5)
        {
            if (StatController.Money >= DmgPrice)
            {
                StatController.Money -= DmgPrice;
                StatController.DamageLvl[selected]++;
                OnUpgradeSuccess();
            }
            else
            {
                Debug.Log("You don't have enough money for Damage upgrade.");
                notEnoughMoneySound.Play();
            }
        }
        else
        {
            Debug.Log("Damage is already at max level.");
        }
    }

    private void OnUpgradeSuccess()
    {
        UpdatePriceTag();

        StatController.Instance.UpdateStats();
        StatController.Instance.UpdateText();
        StatController.Instance.Save();

        powerup.Play();
    }

    public void UpdatePriceTag()
    {
        // Prices for each spaceship (5 levels)
        int[][] spaceshipPrices =
        {
            new int[] { 100, 250, 500, 750, 1000 },
            new int[] { 250, 500, 750, 1000, 1250 },
            new int[] { 500, 750, 1000, 1250, 1500 },
            new int[] { 750, 1000, 1250, 1500, 1750 }
        };

        int selected = StatController.selected;
        int[] prices = spaceshipPrices[selected];

        int dmgLevel = StatController.DamageLvl[selected];
        int fireRateLevel = StatController.FireRateLvl[selected];

        DmgPrice = dmgLevel < 5 ? prices[dmgLevel] : 0;
        FireRatePrice = fireRateLevel < 5 ? prices[fireRateLevel] : 0;

        DmgCostText.text = dmgLevel < 5 ? DmgPrice.ToString() : "";
        DmgLvlText.text = dmgLevel < 5 ? "LVL. " + dmgLevel : "LVL. MAX";

        FireRateCostText.text = fireRateLevel < 5 ? FireRatePrice.ToString() : "";
        FireRateLvlText.text = fireRateLevel < 5 ? "LVL. " + fireRateLevel : "LVL. MAX";

        dmgDisable.SetActive(!(dmgLevel >= 5));
        fireRateDisable.SetActive(!(fireRateLevel >= 5));
    }
}
