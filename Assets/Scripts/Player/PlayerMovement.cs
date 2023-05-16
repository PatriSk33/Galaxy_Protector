using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    public static Vector2 move;
    public GameObject bulletPrefab;
    private float fireRate = StatController.FireRate;
    private float xRange = 30;
    private float cooldown;
    public bool LaserSpaceship;

    //Spaceship
    public GameObject[] spawnpoints;

    //Audio
    public AudioSource shootSound;

    private void Awake()
    {
        InvokeRepeating("Shoot", 1, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for left and right bounds
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        transform.Translate(Vector3.right * Time.deltaTime * 5 * move, Space.World);

        if (LaserSpaceship && cooldown > -1)
        {
            cooldown -= Time.deltaTime;
        }
        if (LaserSpaceship && cooldown <= 0)
        {
                StartCoroutine(ActivateLaser());
        }
    }

    // Called when the left button is pressed
    public void OnButtonDown(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    void Shoot()
    {
        if (StatController.selected != 3)
        {
            for (int i = 0; i < spawnpoints.Length; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, spawnpoints[i].transform.position, Quaternion.identity) as GameObject;
            }
            shootSound.Play();
        }          
    }

    IEnumerator ActivateLaser()
    {
        cooldown = 5;
        bulletPrefab.SetActive(true);
        yield return new WaitForSeconds(3);
        bulletPrefab.SetActive(false);
    }
}
