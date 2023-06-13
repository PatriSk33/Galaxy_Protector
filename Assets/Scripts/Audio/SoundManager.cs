using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource playerGotHitSound, enemyExploded;

    public void Awake()
    {
        instance = this;
    }

    public void PlayOnPlayerHit()
    {
        playerGotHitSound.Play();   
    }

    public void EnemyExploded()
    {
        enemyExploded.Play();
    }
}
