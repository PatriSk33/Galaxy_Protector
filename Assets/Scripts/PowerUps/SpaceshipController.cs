using UnityEngine;
using System.Collections;
using static PowerUp;

public class SpaceshipController : MonoBehaviour
{
    public static SpaceshipController instance;

    [HideInInspector] public bool isSpeedBoostActive = false;
    [HideInInspector] public bool isShieldActive = false;

    public GameObject shieldVisual; // Reference to the shield visual effect object
    public float shieldDuration = 10f; // Duration of the shield power-up

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        shieldVisual.SetActive(false); // Deactivate the shield visual effect initially
    }

    public void ApplySpeedBoost(float duration)
    {
        // Apply speed boost effects
        isSpeedBoostActive = true;
        StartCoroutine(DeactivatePowerUp(duration, PowerUpType.SpeedBoost));
    }

    public void ApplyUltraFireRate(float duration)
    {
        if (PlayerMovement.instance.LaserSpaceship)
        {
            PlayerMovement.instance.cooldownIncrement /= 2; 
        }
        else
        {
            PlayerMovement.instance.CancelInvoke("Shoot");
            PlayerMovement.instance.InvokeRepeating("Shoot", 0, StatController.FireRate / 2);
        }

        StartCoroutine(DeactivatePowerUp(duration, PowerUpType.UltraFireRate));
    }

    public void ActivateShield(float duration)
    {
        if (!isShieldActive)
        {
            isShieldActive = true;
            shieldVisual.SetActive(true); // Activate the shield visual effect
        }

        StartCoroutine(DeactivatePowerUp(duration, PowerUpType.Shield));
    }

    private IEnumerator DeactivatePowerUp(float duration, PowerUpType powerUpType)
    {
        yield return new WaitForSeconds(duration);

        // Deactivate the corresponding power-up effect based on the powerUpType
        switch (powerUpType)
        {
            case PowerUpType.SpeedBoost:
                // Restore the original speed or acceleration values
                isSpeedBoostActive = false;
                break;

            case PowerUpType.UltraFireRate:
                // Restore the original fire rate
                PlayerMovement.instance.CancelInvoke("Shoot");
                PlayerMovement.instance.InvokeRepeating("Shoot", 0, StatController.FireRate);
                break;

            case PowerUpType.Shield:
                // Disable the shield effect or remove the additional health buffer
                isShieldActive = false;
                shieldVisual.SetActive(false); // Deactivate the shield visual effect
                break;
        }
    }
}
