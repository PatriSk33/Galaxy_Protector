using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyController : MonoBehaviour
{
    public float health = 3;
    public GameObject bulletPrefab;
    public float damage;
    public float fireRate = 2;
    private float moveTime = 6;
    public  int amountOfMoney = 5;
    public bool isBoss;

    public bool laserIn;

    public GameObject explosion;

    private void Awake()
    {
        InvokeRepeating("Shoot",moveTime, fireRate);
    }

    private void Update()
    {
        //Death
        if (health <= 0) { Death(); }

        //Make it go forward for a bit and then start shooting
        if (moveTime > 0)
        {
            transform.Translate(Vector3.forward * 3 * Time.deltaTime);
            moveTime -= Time.deltaTime;
        }

        //Behavior if the laser of the 4.spaceship is touching the enemy
        if (laserIn)
        {
            health -= Time.deltaTime * StatController.Damage - 1;
        }

        //Don't make it go up or down
        if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        //Barriers on the front and the back
        if (transform.position.z > 50)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 50);
        }
        if (transform.position.z < 29)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 29);
        }
    }

    void Shoot()
    {
        if (isBoss)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-1,0,0), transform.rotation) as GameObject;
            bullet.GetComponent<GoForward>().damage = damage;
            bullet = Instantiate(bulletPrefab, transform.position + new Vector3(+1, 0, 0), transform.rotation) as GameObject;
            bullet.GetComponent<GoForward>().damage = damage;
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
            bullet.GetComponent<GoForward>().damage = damage;
        }
    }

    void Death()
    {
        SoundManager.instance.EnemyExploded();
        Instantiate(explosion, transform.position, transform.rotation);
        if (StatController.instance.bonus == 1) { 
            //StatController.Money += Mathf.CeilToInt(amountOfMoney + amountOfMoney * 0.2f);
            AfterGameController.addedMoney += Mathf.CeilToInt(amountOfMoney + amountOfMoney * 0.2f);
        }
        else if (StatController.instance.bonus == 0)
        {
            //StatController.Money += amountOfMoney;
            AfterGameController.addedMoney += amountOfMoney;
        }
        EnemySpawner.Instance.enemiesOnField.Remove(this.gameObject);
        if (StatController.selected == 3)
        {
            Laser.instance.colliders.Remove(this.gameObject.GetComponent<Collider>());
        }
        Destroy(gameObject);
        EnemySpawner.Instance.CheckEnemies();
    }
}
