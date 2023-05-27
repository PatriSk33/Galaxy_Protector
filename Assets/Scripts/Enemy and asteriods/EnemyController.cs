using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab;

    [Header("Stats")]
    public float health = 3f;
    public float damage;
    public float fireRate = 2f;
    private float moveTime = 7f;
    private float moveSpeed = 3f;
    public int amountOfMoney = 5;
    public bool isBoss;

    [HideInInspector] public bool laserIn;

    [Header("Effects")]
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
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            moveTime -= Time.deltaTime;
        }

        //Behavior if the laser of the 4.spaceship is touching the enemy
        if (laserIn)
        {
            health -= Time.deltaTime * (StatController.Damage - (StatController.FireRate - 2));
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
        //Barriers Left and Right
        if (transform.position.x < -27)
        {
            transform.position = new Vector3(-27, transform.position.y, transform.position.z);
        }
        if (transform.position.x > 27)
        {
            transform.position = new Vector3(27, transform.position.y, transform.position.z);
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
        //Explosion
        SoundManager.instance.EnemyExploded();
        Instantiate(explosion, transform.position, transform.rotation);

        //Calculate how much money you get
        AfterGameController.addedMoney += amountOfMoney;

        //Remove it from enemies on field
        EnemySpawner.Instance.enemiesOnField.Remove(this.gameObject);

        //Remove this enemy from player laser array
        if (StatController.selected == 3)
        {
            Laser.instance.colliders.Remove(this.gameObject.GetComponent<Collider>());
        }

        //Check if there are more enemies on the field?
        EnemySpawner.Instance.CheckEnemies();

        Destroy(gameObject);
    }
}
