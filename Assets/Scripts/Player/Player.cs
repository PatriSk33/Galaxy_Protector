using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun
{
    public int damageLvl;
    public int fireRateLvl;
    public bool isBuyed;

    public Gun(int damageLvl, int fireRateLvl, bool isBuyed)
    {
        this.damageLvl = damageLvl;
        this.fireRateLvl = fireRateLvl;
        this.isBuyed = isBuyed;
    }
}
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; } 

    public static Vector2 move;
    public static Vector3 curPos;

    private float movementSpeed = 8;
    private float fireRate;
    private float xRange = 30;
    private float cooldown;
    public float cooldownIncrement = 5;
    private bool isLaserSpaceship;

    [Header("Bullets")]
    public GameObject[] spawnpoints;
    public GameObject bulletPrefab;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] shootSounds;

    public Gun ReturnClass(int i)
    {
        return new Gun(StatController.DamageLvl[i], StatController.FireRateLvl[i], ShopController.buyed[i]);
    }

    public void SetStats(Gun gun,int i)
    {
        StatController.DamageLvl[i] = gun.damageLvl;
        StatController.FireRateLvl[i] = gun.fireRateLvl;
        ShopController.buyed[i] = gun.isBuyed;
    }

    private void Awake()
    {
        Instance = this;

        
        if (!isLaserSpaceship)
        {
            fireRate = StatController.FireRate;
            InvokeRepeating("Shoot", 1, fireRate);
        }
        else 
        {
            cooldownIncrement = StatController.FireRate + 3;
            cooldown = Time.time + cooldownIncrement;
        }
    }

    void Update()
    {
        curPos = transform.position;

        CheckBarriers();

        HandleMovement();

        // Laser ship
        if (isLaserSpaceship && cooldown < Time.time)
        {
            StartCoroutine(ActivateLaser());
        }
    }

    private void CheckBarriers()
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
    }

    private void HandleMovement()
    {
        // Move left and Right
        if (!SpaceshipController.Instance.isSpeedBoostActive)
        {
            transform.Translate(Vector3.right * Time.deltaTime * movementSpeed * move, Space.World);
        }
        else
        {
            float speedBoostMultiplier = 1.2f;
            transform.Translate(Vector3.right * Time.deltaTime * movementSpeed * move * speedBoostMultiplier, Space.World);  //Speed boost activated
        }
    }

    public void Shoot()
    {
        if (StatController.selected != 3)
        {
            // Spaceship is not a laser one

            for (int i = 0; i < spawnpoints.Length; i++)
            {
                Instantiate(bulletPrefab, spawnpoints[i].transform.position, Quaternion.identity);
            }

            // Randomly select a shoot sound from the array
            AudioClip randomShootSound = shootSounds[Random.Range(0, shootSounds.Length)];
            // Play the selected shoot sound
            audioSource.PlayOneShot(randomShootSound);
        }          
    }

    IEnumerator ActivateLaser()
    {
        cooldown = Time.time + cooldownIncrement;
        bulletPrefab.SetActive(true);
        yield return new WaitForSeconds(3);
        bulletPrefab.SetActive(false);
    }

    public bool IsLaserSpaceship()
    {
        return isLaserSpaceship;
    }
}
