using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoForward : MonoBehaviour
{
    private float lifeTime = 3;
    public int bulletHealth = 2;
    public bool isPlayerBullet, isBossBullet;
    [HideInInspector] public float damage;

    void Update()
    {
        transform.Translate(Vector3.forward * 15 * Time.deltaTime);
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
        if (bulletHealth == 0) { Destroy(gameObject); }
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
            SoundManager.instance.PlayOnPlayerHit();
            StatController.Health -= damage;
            StatController.CheckHealth();
            Destroy(gameObject);
        }

        if (col.tag == "Bullet" && isPlayerBullet && !col.gameObject.GetComponent<GoForward>().isPlayerBullet && !col.gameObject.GetComponent<GoForward>().isBossBullet)
        {
            bulletHealth--;
            Destroy(col.gameObject);
        }
    }
}
