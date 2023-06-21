using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            GoForward bulletController = other.GetComponent<GoForward>();
            if (bulletController != null && !bulletController.isPlayerBullet)
            {
                Destroy(other.gameObject); // Destroy the enemy bullet
            }
        }
        else if ( other.CompareTag("Asteroid"))
        {
            Destroy(other.gameObject); // Destroy Asteroid
        }
    }
}
