using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthMiddleMan : MonoBehaviour
{
    public Text healthText;
    void Update()
    {
        healthText.text = StatController.Health.ToString();
    }
}
