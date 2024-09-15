using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Variables
    public GameObject road;
    public GameObject collectible;
    public GameObject obstacles;
    private GameObject player;

    public GameObject titleScreen;
    public GameObject gameScreen;
    public GameObject gameOverScreen;

    private Vector3 playerLastPosition;

    private float timeSinceLastObstacleSpawn;
    private float timeSinceLastCollectibleSpawn;
    public float obstacleSpawnRate = 3;
    public float collectibleSpawnRate = 0.5f;

    public bool isGameActive;

    // Start and Update
    void Start()
    {
        player = GameObject.Find("Player");
        playerLastPosition = player.transform.position;
        isGameActive = false;
        Time.timeScale = 0;
    }

    void Update()
    {
        timeSinceLastObstacleSpawn += Time.deltaTime;
        timeSinceLastCollectibleSpawn += Time.deltaTime;

        if (timeSinceLastObstacleSpawn > obstacleSpawnRate)
        {
            SpawnObstacle();
            timeSinceLastObstacleSpawn = 0;
        }

        if (timeSinceLastCollectibleSpawn > collectibleSpawnRate)
        {
            SpawnCollectible();
            timeSinceLastCollectibleSpawn = 0;
        }

        if (player.transform.position.z > playerLastPosition.z + 100)
        {
            SpawnRoad();
            playerLastPosition = player.transform.position;
        }
    }

    // Methods
    void SpawnCollectible()
    {
        Instantiate(collectible, GenerateCollectibleSpawnPoint(), collectible.transform.rotation);
    }

    void SpawnObstacle()
    {
        Instantiate(obstacles, GenerateObstacleSpawnPoint(), obstacles.transform.rotation);
    }

    void SpawnRoad()
    {
        Instantiate(road, new(0, 0, player.transform.position.z + 101.75f), road.transform.rotation);
    }

    public void StartGame()
    {
        isGameActive = true;
        titleScreen.SetActive(false);
        gameScreen .SetActive(true);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        isGameActive = false;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    Vector3 GenerateObstacleSpawnPoint()
    {
        float[] optionsX = {-3f, 0, 3f,};
        float randomX = optionsX[Random.Range(0, optionsX.Length)];
        float spawnZ = player.transform.position.z + 50f;
        Vector3 spawnPoint = new(randomX, 0, spawnZ);
        return spawnPoint;
    }

    Vector3 GenerateCollectibleSpawnPoint()
    {
        float[] optionsX = {-3f, 0, 3f,};
        float randomX = optionsX[Random.Range(0, optionsX.Length)];
        float spawnZ = player.transform.position.z + 50f;
        Vector3 spawnPoint = new(randomX, 1, spawnZ);
        return spawnPoint;
    }
}
