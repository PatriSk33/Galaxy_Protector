using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    public List<GameObject> stonesPrefabs;
    public List<GameObject> enemyPrefabsEasy; // Array of enemy prefabs to spawn
    public List<GameObject> enemyPrefabsMedium; // Array of enemy prefabs to spawn
    public List<GameObject> enemyPrefabsHard; // Array of enemy prefabs to spawn
    public List<GameObject> enemiesOnField;
    public float spawnDelay = 2f; // Delay between enemy spawns
    public float spawnDelayAsteroids = 10f; // Delay between enemy spawns
    public int maxEnemies; // Maximum number of enemies to spawn

    private int numEnemiesSpawned = 0;
    private bool waited = false;
    private bool longwaited = false;

    private int xRange = 27;

    // UFO types
    public GameObject bossPrefab;

    public bool spawnedBoss = false;

    private void Awake()
    {
        Instance = this;
        UpdateMaxEnemiesCount();
    }
    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnDelay, spawnDelay);
        InvokeRepeating("SpawnAsteroids", spawnDelayAsteroids, spawnDelayAsteroids);
    }

    void SpawnEnemy()
    {
        if (numEnemiesSpawned >= maxEnemies)
        {
            return;
        }

        int xP = Random.Range(-xRange, xRange); // Choose random X position 

        GameObject enemy = null;
        if (StatController.Wave == 10 && numEnemiesSpawned == 39 && !spawnedBoss)
        {
            enemy = Instantiate(bossPrefab, new Vector3(xP, 0, 50), new Quaternion(0, 180, 0, 0));
            spawnedBoss = true;
        }
        else if (StatController.Wave >= 7 && numEnemiesSpawned != 39)
        {
            int prefabIndex = Random.Range(0, enemyPrefabsMedium.Count); // Choose a random enemy prefab from the array
            enemy = Instantiate(enemyPrefabsHard[prefabIndex], new Vector3(xP, 0, 50), new Quaternion(0, 180, 0, 0));
        }
        else if (StatController.Wave >= 4)
        {
            int prefabIndex = Random.Range(0, enemyPrefabsMedium.Count); // Choose a random enemy prefab from the array
            enemy = Instantiate(enemyPrefabsMedium[prefabIndex], new Vector3(xP, 0, 50), new Quaternion(0, 180, 0, 0));
        }
        else if (StatController.Wave <= 3)
        {
            int prefabIndex = Random.Range(0, enemyPrefabsEasy.Count); // Choose a random enemy prefab from the array
            enemy = Instantiate(enemyPrefabsEasy[prefabIndex], new Vector3(xP, 0, 50), new Quaternion(0, 180, 0, 0));
        }
        
        numEnemiesSpawned++;

        enemiesOnField.Add(enemy);

        if (numEnemiesSpawned > maxEnemies / 2 && StatController.Wave > 3 && waited == false)
        {
            StartCoroutine("Wait");
            longwaited = false;
        }
        else if (numEnemiesSpawned > maxEnemies / 1.1f && StatController.Wave > 8 && longwaited == false)
        {
            StartCoroutine("Wait");
        }

    }
    void SpawnAsteroids()
    {
        int Index = Random.Range(0, stonesPrefabs.Count);

        int xP = Random.Range(-17, 17); // Choose random X position

        GameObject stones = Instantiate(stonesPrefabs[Index], new Vector3(xP, 0, 100), new Quaternion(0, 180, 0, 0));
    }

    IEnumerator Wait()
    {
        CancelInvoke("SpawnEnemy");
        waited = true;
        longwaited = true;
        yield return new WaitForSeconds(30);
        InvokeRepeating("SpawnEnemy", spawnDelay, spawnDelay);
    }

    public void CheckEnemies()
    {
        if (enemiesOnField.Count == 0 && numEnemiesSpawned == maxEnemies)
        {
            waited = false;
            longwaited = false;
            if (StatController.Wave < 11)
            {
                StatController.Wave++;
            }
            StatController.instance.Save();
            AfterGameController.won = true;
            AfterGameController.afterGame = true;
            SceneManager.LoadScene(0);
        }
    }

    private void UpdateMaxEnemiesCount()
    {
        switch (StatController.Wave)
        {
            case 0:
                maxEnemies = 5;
                spawnDelayAsteroids = 10f;
                break;
            case 1:
                maxEnemies = 7;
                spawnDelayAsteroids = 10f;
                break;
            case 2:
                maxEnemies = 10;
                spawnDelayAsteroids = 9f;
                break;
            case 3:
                maxEnemies = 14;
                spawnDelayAsteroids = 9f;
                break;
            case 4:
                maxEnemies = 16;
                spawnDelayAsteroids = 8f;
                break;
            case 5:
                maxEnemies = 19;
                spawnDelayAsteroids = 8f;
                break;
            case 6:
                maxEnemies = 21;
                spawnDelayAsteroids = 7f;
                break;
            case 7:
                maxEnemies = 25;
                spawnDelayAsteroids = 7f;
                break;
            case 8:
                maxEnemies = 30;
                spawnDelayAsteroids = 6f;
                break;
            case 9:
                maxEnemies = 35;
                spawnDelayAsteroids = 5f;
                break;
            case 10:
                maxEnemies = 40;
                spawnDelayAsteroids = 4f;
                spawnedBoss = false;
                break;
        }
    }
}