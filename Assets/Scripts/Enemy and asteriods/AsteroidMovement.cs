using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    private float lifeTime = 4;
    public float damage;

    private Vector3 rotationDirection;
    private float minSpeed = 5f;
    private float maxSpeed = 10f;
    private float rotationSpeed;

    //Camera Shake
    public float shakeDuration = 0.2f;
    public float shakeIntensity = 0.1f;

    private Camera mainCamera;
    private Vector3 originalCameraPosition;
    private bool isShaking = false;

    [Header("Effects")]
    public GameObject explosion;


    void Start()
    {
        mainCamera = Camera.main;
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
        if (other.CompareTag("Player"))
        {
            SoundManager.instance.PlayOnPlayerHit(); //Player got hit sound
            
            //Shake camera
            if (!isShaking)
            {
                StartCoroutine(ShakeCamera());
            }

            //Damage player
            StatController.Health -= damage;
            if (StatController.Health <= 0)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                other.gameObject.SetActive(false);
            }

            Destroy(gameObject, 1); //Destory Asteroid
        }
    }

    private IEnumerator ShakeCamera()
    {
        isShaking = true;

        originalCameraPosition = mainCamera.transform.position;
        float timeElapsed = 0f;

        while (timeElapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;
            mainCamera.transform.position = originalCameraPosition + randomOffset;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalCameraPosition;
        isShaking = false;
    }
}
