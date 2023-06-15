using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUpPrefabs; // Array of power-up prefabs
    public float spawnInterval = 15f; // Time interval between power-up spawns
    private int xRange = 27; //Range where can powerups spawn on X axis

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPowerUp();
            timer = 0f;
        }
    }

    private void SpawnPowerUp()
    {
        int randomIndex = Random.Range(0, powerUpPrefabs.Length);
        GameObject powerUpPrefab = powerUpPrefabs[randomIndex];     // Get prefab to spawn

        Vector3 spawnPosition = new Vector3(Random.Range(-xRange, xRange), 0, 65); // Calculate position

        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);     // Spawn the powerUp
    }
}
