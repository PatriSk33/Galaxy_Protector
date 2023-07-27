using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            BulletController bulletController = other.GetComponent<BulletController>();
            if (bulletController != null && !bulletController.IsPlayerBullet())
            {
                // Its Enemy Bullet
                Destroy(other.gameObject);
            }
        }
        else if ( other.CompareTag("Asteroid"))
        {
            Destroy(other.gameObject); // Destroy Asteroid
        }
    }
}
