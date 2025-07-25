using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab;

    [Header("Stats")]
    public float health = 3f;
    public float damage;
    public float fireRate = 2f;
    private float moveTime = 7f;
    private float moveSpeed = 3f;
    public int amountOfMoney;
    [Tooltip("Bullets spawned")][Range(1,7)]public int enemyIndex;

    [HideInInspector] public bool laserIn;

    [Header("Effects")]
    public GameObject explosion;

    private float laserTime;

    private void Awake()
    {
        InvokeRepeating("Shoot",moveTime, fireRate);
        laserTime = Time.time + StatController.FireRate;
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
            if (Time.time > laserTime)
            {
                health -= StatController.Damage;
                laserTime = Time.time + StatController.FireRate;
            }
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
        GameObject bullet = null;
        switch (enemyIndex)
        {
            case 1:
                bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                break;
            case 2:
                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-1,0,0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(+1, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                break;
            case 3:
                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.2f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, -4, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.2f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, 4, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                break;
            case 4:
                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.2f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, -4, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.2f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, 4, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.25f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, -8, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.25f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, 8, 0);
                break;
            case 5:
                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.2f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, -4, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.2f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, 4, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.25f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, -8, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.25f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, 8, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                break;
            case 6:
                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.2f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, -4, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.2f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, 4, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.25f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, -8, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.25f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, 8, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.3f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(+0.3f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                break;
            case 7:
                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.2f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, -4, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.2f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, 4, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.25f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, -8, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0.25f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                bullet.transform.Rotate(0, 8, 0);

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-0.3f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(+0.3f, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;

                bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, 0), transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = damage;
                break;
            default:
                Debug.LogError("Wrong enemyIndex set!");
                break;
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
            Laser.Instance.colliders.Remove(this.gameObject.GetComponent<Collider>());
        }

        //Leaderboard
        StatController.enemiesKilled++;

        //Check if there are more enemies on the field?
        EnemySpawner.Instance.CheckEnemies();

        Destroy(gameObject);
    }
}
