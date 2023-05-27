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
public class PlayerMovement : MonoBehaviour
{
    public static Vector2 move;
    public static Vector3 curPos;

    private float fireRate;
    private float xRange = 30;
    private float cooldown;
    public bool LaserSpaceship;

    //Spaceship
    public GameObject[] spawnpoints;
    public GameObject bulletPrefab;

    //Audio
    public AudioSource shootSound;

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
        fireRate = StatController.FireRate;
        if (!LaserSpaceship)
        {
            InvokeRepeating("Shoot", 1, fireRate);
        }
        else { cooldown = Time.time + 5; }
    }

    // Update is called once per frame
    void Update()
    {
        curPos = transform.position;

        // Check for left and right bounds
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        // Move left and Right
        transform.Translate(Vector3.right * Time.deltaTime * 5 * move, Space.World);

        // Laser ship
        if (LaserSpaceship && cooldown < Time.time)
        {
            StartCoroutine(ActivateLaser());
        }
    }

    void Shoot()
    {
        if (StatController.selected != 3)
        {
            for (int i = 0; i < spawnpoints.Length; i++)
            {
                Instantiate(bulletPrefab, spawnpoints[i].transform.position, Quaternion.identity);
            }
            shootSound.Play();
        }          
    }

    IEnumerator ActivateLaser()
    {
        cooldown = Time.time + 5;
        bulletPrefab.SetActive(true);
        yield return new WaitForSeconds(3);
        bulletPrefab.SetActive(false);
    }
}
