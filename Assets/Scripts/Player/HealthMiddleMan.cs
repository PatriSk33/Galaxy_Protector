using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMiddleMan : MonoBehaviour
{
    public Text healthText;
    public Image energyBar;

    void Update()
    {
        UpdateEnergyBar();
    }

    public void UpdateEnergyBar()
    {
        float healthPercentage = StatController.Health / StatController.MaxHealth;
        Color targetColor = Color.Lerp(Color.green, Color.red, 1 - healthPercentage);
        energyBar.color = targetColor;

        energyBar.fillAmount = healthPercentage;

        healthText.text = $"{(int)StatController.Health}/{(int)StatController.MaxHealth}";
    }
}
