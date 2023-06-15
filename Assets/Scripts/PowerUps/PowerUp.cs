using UnityEditor.Rendering;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        SpeedBoost,
        UltraFireRate,
        Shield
    }

    public PowerUpType powerUpType;
    public float powerUpDuration = 8f;
    public float speed = 6f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivatePowerUp(other.gameObject);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        if(transform.position.z < -2)
        {
            Destroy(gameObject);
        }
    }

    private void ActivatePowerUp(GameObject player)
    {
        // Get the player's spaceship controller script
        SpaceshipController spaceshipController = player.GetComponent<SpaceshipController>();

        // Activate the corresponding power-up effect based on the powerUpType
        switch (powerUpType)
        {
            case PowerUpType.SpeedBoost:
                spaceshipController.ApplySpeedBoost(powerUpDuration);
                break;

            case PowerUpType.UltraFireRate:
                spaceshipController.ApplyUltraFireRate(powerUpDuration);
                break;

            case PowerUpType.Shield:
                spaceshipController.ActivateShield(powerUpDuration);
                break;
        }
    }
}
