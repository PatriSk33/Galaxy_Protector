using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Random things to shoot at player")]
    public List<GameObject> randomThingsPrefabs;
    public GameObject dangerMark;

    [Header("Enemies in waves")]
    [Tooltip("0 - 5")] public List<GameObject> enemyPrefabs5;
    [Tooltip("5 - 10")] public List<GameObject> enemyPrefabs10;
    [Tooltip("10 - 15")] public List<GameObject> enemyPrefabs15;
    [Tooltip("15 - 20")] public List<GameObject> enemyPrefabs20;
    [Tooltip("20 - 25")] public List<GameObject> enemyPrefabs25;
    [Tooltip("25 - 30")] public List<GameObject> enemyPrefabs30;
    [Tooltip("30 - 35")] public List<GameObject> enemyPrefabs35;
    [Tooltip("35 - 40")] public List<GameObject> enemyPrefabs40;
    [Tooltip("40 - 45")] public List<GameObject> enemyPrefabs45;
    [Tooltip("45 - 50")] public List<GameObject> enemyPrefabs50;
    [Tooltip("50 - 55")] public List<GameObject> enemyPrefabs55;
    [Tooltip("55 - 60")] public List<GameObject> enemyPrefabs60;
    [Tooltip("60 - 65")] public List<GameObject> enemyPrefabs65;
    [Tooltip("65 - 70")] public List<GameObject> enemyPrefabs70;
    [Tooltip("70 - 75")] public List<GameObject> enemyPrefabs75;
    [Tooltip("75 - 80")] public List<GameObject> enemyPrefabs80;
    [Tooltip("80 - 85")] public List<GameObject> enemyPrefabs85;
    [Tooltip("85 - 90")] public List<GameObject> enemyPrefabs90;
    [Tooltip("90 - 95")] public List<GameObject> enemyPrefabs95;
    [Tooltip("95 - 100")] public List<GameObject> enemyPrefabs100;
    List<GameObject>[] enemyPrefabs = new List<GameObject>[20];


    [HideInInspector]public List<GameObject> enemiesOnField;

    private float spawnDelay = 2f; // Delay between enemy spawns
    private float spawnDelayRandomThings = 13f; // Delay between random things spawns
    private int xRange = 30; //Range where can enemies spawn on X axis
    private int numEnemiesSpawned = 0; //How many Enemies are already spawned
    private int maxEnemies; // Maximum number of enemies to spawn

    //Boss
    [Header("Boss")]
    public GameObject[] bossPrefab;

    private void Awake()
    {
        Instance = this;
        UpdateMaxEnemiesCount();

        enemyPrefabs[0] = enemyPrefabs5;
        enemyPrefabs[1] = enemyPrefabs10;
        enemyPrefabs[2] = enemyPrefabs15;
        enemyPrefabs[3] = enemyPrefabs20;
        enemyPrefabs[4] = enemyPrefabs25;
        enemyPrefabs[5] = enemyPrefabs30;
        enemyPrefabs[6] = enemyPrefabs35;
        enemyPrefabs[7] = enemyPrefabs40;
        enemyPrefabs[8] = enemyPrefabs45;
        enemyPrefabs[9] = enemyPrefabs50;
        enemyPrefabs[10] = enemyPrefabs55;
        enemyPrefabs[11] = enemyPrefabs60;
        enemyPrefabs[12] = enemyPrefabs65;
        enemyPrefabs[13] = enemyPrefabs70;
        enemyPrefabs[14] = enemyPrefabs75;
        enemyPrefabs[15] = enemyPrefabs80;
        enemyPrefabs[16] = enemyPrefabs85;
        enemyPrefabs[17] = enemyPrefabs90;
        enemyPrefabs[18] = enemyPrefabs95;
        enemyPrefabs[19] = enemyPrefabs100;
    }
    private void Start()
    {
        StartCoroutine(SpawnEnemy());
        InvokeRepeating("SpawnRandomThings", spawnDelayRandomThings - 1.5f, spawnDelayRandomThings - 1.5f);
    }

    private void Update()
    {
        if (enemiesOnField.Count == 0)
        {
            StopCoroutine("Wait");
            StartCoroutine(SpawnEnemy());
        }
    }

    IEnumerator SpawnEnemy()
    {
        int xP = Random.Range(-xRange, xRange);
        GameObject enemy = null;
        GameObject enemyObject = null;
        int prefabIndex;

        if (GetNumOfEnemiesSpawned() >= GetMaxEnemiesAmount())
        {
            yield break;
        }

        if (StatController.Wave <= 100)
        {
            int waveIndex = (StatController.Wave - 1) / 5;
            if (waveIndex < enemyPrefabs.Length)
            {
                List<GameObject> waveEnemyPrefabs = enemyPrefabs[waveIndex];
                prefabIndex = Random.Range(0, waveEnemyPrefabs.Count);
                enemy = waveEnemyPrefabs[prefabIndex];
            }
        }
        else
        {
            yield break;
        }

        // Check if the current wave is a boss wave
        bool isBossWave = (StatController.Wave % 10 == 0) && (GetNumOfEnemiesSpawned() == GetMaxEnemiesAmount() - 1);
        if (isBossWave)
        {
            StartCoroutine(Wait(60, true));
            yield break;
        }

        enemyObject = Instantiate(enemy, new Vector3(xP, 0, 49), enemy.transform.rotation);

        if (enemy != null)
        {
            IncreaseNumOfEnemiesSpawned();
            enemiesOnField.Add(enemyObject);
        }

        if (!isBossWave && GetNumOfEnemiesSpawned() % 8 == 0 && GetNumOfEnemiesSpawned() > 0)
        {
            StartCoroutine(Wait(45, false));
        }
        else
        {
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(SpawnEnemy());
        }
    }

    void SpawnRandomThings()
    {
        StartCoroutine(SpawnRandomThing());
    }

    IEnumerator SpawnRandomThing()
    {
        // Danger Mark
        dangerMark.SetActive(true);         // Activate
        yield return new WaitForSeconds(1);
        dangerMark.SetActive(false);        // Deactivate
        yield return new WaitForSeconds(0.5f);

        // Get index and X position
        int Index = Random.Range(0, randomThingsPrefabs.Count);
        int xP = Random.Range(-xRange, xRange);

        // Spawn
        Instantiate(randomThingsPrefabs[Index], new Vector3(xP, -1.5f, 70), new Quaternion(0, 180, 0, 0));
    }

    IEnumerator Wait(float waitTime, bool isBossWave)
    {
        if (enemiesOnField.Count > 0)
        {
            yield return new WaitForSeconds(waitTime);
        }

        if (isBossWave)
        {
            int bossIndex = (StatController.Wave / 10) - 1;
            GameObject enemy = bossPrefab[bossIndex];
            GameObject enemyObject = Instantiate(enemy, new Vector3(Random.Range(-xRange, xRange), 0, 65), enemy.transform.rotation);

            if (enemy != null)
            {
                numEnemiesSpawned++;
                enemiesOnField.Add(enemyObject);
            }

            yield break;
        }

        StartCoroutine(SpawnEnemy());
    }

    public void CheckEnemies()
    {
        if (enemiesOnField.Count == 0 && numEnemiesSpawned >= maxEnemies)
        {
            if (StatController.Wave == StatController.WaveCompleted)
            {
                if (StatController.WaveCompleted < 100)
                {
                    StatController.WaveCompleted++;
                }
            }

            StatController.Instance.Save();
            AfterGameController.won = true;
            AfterGameController.Instance.ShowPanel();
        }
    }

    private void UpdateMaxEnemiesCount()
    {
        numEnemiesSpawned = 0;

        if (StatController.Wave < 100 && StatController.Wave >= 1)
        {
            maxEnemies = 1 + StatController.Wave;
            spawnDelayRandomThings = 15 - (StatController.Wave - 1) / 10;
        }
        else if (StatController.Wave == 100)
        {
            maxEnemies = 150;
            spawnDelayRandomThings = 5;
        }
    }

    public int GetMaxEnemiesAmount()
    {
        return maxEnemies;
    }

    public int GetNumOfEnemiesSpawned()
    {
        return numEnemiesSpawned;
    }

    public void IncreaseNumOfEnemiesSpawned()
    {
        numEnemiesSpawned++;
    }

}