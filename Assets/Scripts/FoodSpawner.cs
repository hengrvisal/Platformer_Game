// FoodSpawner.cs
using UnityEngine;
using System.Collections.Generic;

public class FoodSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] foodPrefabs;
    public int maxFoodItems = 5;
    public float spawnInterval = 3f;
    public Transform[] spawnPoints;

    [Header("Pooling")]
    private List<GameObject> foodPool = new List<GameObject>();

    private float spawnTimer;

    private void Start()
    {
        InitializePool();
        SpawnInitialFood();
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && GetActiveFoodCount() < maxFoodItems)
        {
            SpawnFood();
            spawnTimer = 0f;
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < maxFoodItems * 2; i++)
        {
            if (foodPrefabs.Length > 0)
            {
                GameObject food = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Length)]);
                food.SetActive(false);
                foodPool.Add(food);
            }
        }
    }

    private void SpawnInitialFood()
    {
        for (int i = 0; i < maxFoodItems; i++)
        {
            SpawnFood();
        }
    }

    private void SpawnFood()
    {
        GameObject food = GetPooledFood();
        if (food != null && spawnPoints.Length > 0)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            food.transform.position = spawnPoint.position;
            food.SetActive(true);
        }
    }

    private GameObject GetPooledFood()
    {
        foreach (GameObject food in foodPool)
        {
            if (!food.activeInHierarchy)
            {
                return food;
            }
        }

        // If no inactive food, create new one
        if (foodPrefabs.Length > 0)
        {
            GameObject newFood = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Length)]);
            foodPool.Add(newFood);
            return newFood;
        }

        return null;
    }

    private int GetActiveFoodCount()
    {
        int count = 0;
        foreach (GameObject food in foodPool)
        {
            if (food.activeInHierarchy) count++;
        }
        return count;
    }
}