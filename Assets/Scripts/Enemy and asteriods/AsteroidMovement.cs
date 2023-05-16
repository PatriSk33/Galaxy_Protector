using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class AsteroidMovement : MonoBehaviour
{
    private float lifeTime = 4;
    public float damage;

    private Vector3 rotationDirection;
    private float minSpeed = 5f;
    private float maxSpeed = 10f;
    private float rotationSpeed;


    void Start()
    {
        rotationSpeed = Random.Range(minSpeed, maxSpeed);
        rotationDirection = new Vector3(Random.Range(0,100), Random.Range(0, 100), Random.Range(0, 100));
    }

    void Update()
    {
        transform.Translate(-Vector3.forward * 40 * Time.deltaTime, Space.World);
        transform.Rotate(rotationDirection * Time.deltaTime * rotationSpeed);
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SoundManager.instance.PlayOnPlayerHit();
            StatController.Health -= damage;
            StatController.CheckHealth();
            Destroy(gameObject);
        }
    }
}
