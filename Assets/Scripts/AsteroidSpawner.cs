using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] asteroidPrefab;
    [SerializeField] float secondsBetweenAsteroids;
    [SerializeField] Vector2 forceRange;

    Camera mainCamera;
    float timer;

    void Start() 
    {
        mainCamera = Camera.main;
    }

    void Update() 
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnAsteroid();

            timer += secondsBetweenAsteroids;
        }
    }

    void SpawnAsteroid()
    {
        int spawnSide = Random.Range(0, 4);

        Vector2 spawnPoint = Vector2.zero;
        Vector2 direction = Vector2.zero;

        switch (spawnSide)
        {
            case 0:
                spawnPoint.x = 0;
                spawnPoint.y = Random.value;
                direction = new Vector2(1f, Random.Range(-1f, 1f));
                break;
            case 1:
                spawnPoint.x = 1;
                spawnPoint.y = Random.value;
                direction = new Vector2(-1f, Random.Range(-1f, 1f));
                break;
            case 2:
                spawnPoint.y = 0;
                spawnPoint.x = Random.value;
                direction = new Vector2(Random.Range(-1f, 1f), 1f);
                break;
            case 3:
                spawnPoint.y = 1;
                spawnPoint.x = Random.value;
                direction = new Vector2(Random.Range(-1f, 1f), -1f);
                break;
        }

        Vector3 worldSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldSpawnPoint.z = 0f;

        GameObject selectedAsteroid = asteroidPrefab[Random.Range(0, asteroidPrefab.Length)];

        GameObject asteroidInstance = Instantiate(
            selectedAsteroid, 
            worldSpawnPoint, 
            Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>();

        rb.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);
    }
}
