using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForward : MonoBehaviour
{
    private float lifeTime = 3;
    [HideInInspector]public float bulletHealth = 2;
    public bool isPlayerBullet, isBossBullet;
    [HideInInspector]public float damage;   // Enemy Damage

    [Serializable]
    public struct HomingBullet
    {
        public bool isHoming;
        public float range;
    }
    public HomingBullet HM;

    private void Start()
    {
        if (HM.isHoming)
        {
            if (Vector3.Distance(transform.position, PlayerMovement.curPos) < HM.range)
            {
                transform.LookAt(PlayerMovement.curPos);
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

        if (col.tag == "Player" && !isPlayerBullet)
        {
            //Effects
            SoundManager.instance.PlayOnPlayerHit();

            //Damage player
            StatController.Health -= damage;
            if (StatController.Health <= 0 && GameplayUIButtons.instance.canRevive)
            {
                GameplayUIButtons.instance.OpenRevivePanel();
            }
            else if (StatController.Health <= 0 && !GameplayUIButtons.instance.canRevive)
            {
                GameplayUIButtons.instance.OpenAfterGame();
            }

            Destroy(gameObject);
        }

        if (col.tag == "Bullet" && isPlayerBullet && !col.gameObject.GetComponent<GoForward>().isPlayerBullet && !col.gameObject.GetComponent<GoForward>().isBossBullet)
        {
            col.GetComponent<GoForward>().bulletHealth -= StatController.Damage;
            bulletHealth -= col.GetComponent<GoForward>().damage;
        }
    }
}
