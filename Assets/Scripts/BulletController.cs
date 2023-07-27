using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float lifeTime = 4.5f;
    public bool isPlayerBullet, isBossBullet;
    [HideInInspector] public float bulletHealth = 2;
    [HideInInspector] public float damage;   // Enemy Damage

    [Serializable] public struct HomingBullet
    {
        public bool isHoming;
        public float range;
    }
    public HomingBullet HM;

    [Header("Effects")]
    public GameObject explosion;

    private void Start()
    {
        if (HM.isHoming)
        {
            if (Vector3.Distance(transform.position, Player.curPos) < HM.range)
            {
                transform.LookAt(Player.curPos);
            }
            HM.isHoming = false;
        }
    }

    void Update()
    {
        transform.Translate(Vector3.forward * 15 * Time.deltaTime);

        //Death
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }

        if (bulletHealth <= 0) { Destroy(gameObject); }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy" && isPlayerBullet)
        {
            col.gameObject.GetComponent<EnemyController>().health -= StatController.Damage;
            Destroy(gameObject);
        }
        else if (col.tag == "Player" && !isPlayerBullet)
        {
            //Effects
            SoundManager.instance.PlayOnPlayerHit();

            //Damage player
            StatController.Health -= damage;
            if (StatController.Health <= 0)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                col.gameObject.SetActive(false);
            }

            Destroy(gameObject);
        }
        else if (col.tag == "Bullet" && isPlayerBullet && !col.gameObject.GetComponent<BulletController>().isPlayerBullet && !col.gameObject.GetComponent<BulletController>().isBossBullet)
        {
            col.GetComponent<BulletController>().bulletHealth -= StatController.Damage;
            bulletHealth -= col.GetComponent<BulletController>().damage;
        }
    }

    public bool IsPlayerBullet()
    {
        return isPlayerBullet;
    }

    public bool IsBossBullet()
    {
        return isBossBullet;
    }
}
