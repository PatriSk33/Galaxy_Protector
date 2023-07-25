using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public static Laser instance;
    public List<Collider> colliders = new List<Collider>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().laserIn = true;
            colliders.Add(other);
        }
        else if(other.CompareTag("Asteroid"))
        {
            Destroy(other.gameObject);
        }
        if (other.tag == "Bullet" && !other.gameObject.GetComponent<GoForward>().isBossBullet)
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyController>().laserIn = false;
            colliders.Remove(other);
            Debug.Log("Disabled");
        }
    }

    void OnDisable()
    {
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<EnemyController>().laserIn = false;
        }

        // Clear the list of colliders
        colliders.Clear();
    }

    private void Awake()
    {
        instance = this;
    }
}
