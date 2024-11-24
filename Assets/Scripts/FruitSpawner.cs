using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitSpawner : MonoBehaviour
{
    public int maxCount = 6;
    public int spawnCount = 2;
    [SerializeField] private GameObject[] fruitPrefabs;
    [SerializeField] private RectTransform[] spawnPoints;
    [SerializeField] private List<GameObject> spawnedFruits;
    [SerializeField] private RectTransform[] pointsInBasket;

    public int Sum { get; private set; }

    void Start()
    {
        spawnedFruits = new List<GameObject>();
        Sum = 0;
        SpawnFruits();
    }

    public void SpawnFruits()
    {
        // Удаляем все созданные фрукты и их дочерние объекты из точек спавна
        foreach (RectTransform spawnPoint in spawnPoints)
        {
            foreach (Transform child in spawnPoint)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        
        spawnedFruits.Clear();
        
        // Создаем список для хранения уже использованных точек спавна
        List<RectTransform> usedSpawnPoints = new List<RectTransform>();

        for (int i = 0; i < spawnCount; i++)
        {
            // Проверяем, есть ли еще доступные точки спавна
            if (usedSpawnPoints.Count >= spawnPoints.Length)
            {
                Debug.LogWarning("No more available spawn points!");
                break;
            }

            // Выбираем случайный префаб фрукта из массива
            GameObject fruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];

            // Выбираем случайную точку спавна из массива, которая еще не использовалась
            RectTransform spawnPoint = null;
            do
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            } while (usedSpawnPoints.Contains(spawnPoint));
            usedSpawnPoints.Add(spawnPoint);

            // Создаем новый фрукт в случайной точке спавна
            GameObject newFruit = Instantiate(fruitPrefab, new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f), Quaternion.identity, spawnPoint);

            // Устанавливаем рандомное вращение
            float randomRotation = Random.Range(0f, 360f);
            newFruit.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);

            float randomScale = Random.Range(0.5f, 1f);
            newFruit.transform.localScale = new Vector3(randomScale, randomScale, 1f);

            // Добавляем созданный фрукт в список созданных фруктов
            spawnedFruits.Add(newFruit);
        }
        
        CalculateSum();

        if (Sum < 0 || Sum >= 100)
        {
            SpawnFruits();
        }
    }

    public void IncreaseSpawnCount()
    {
        spawnCount++;

        if (spawnCount > maxCount)
        {
            spawnCount = maxCount;
        }
    }

    private void CalculateSum()
    {
        string formula = string.Empty;
        Sum = 0;
        foreach (RectTransform spawnPoint in spawnPoints)
        {
            if (spawnPoint.childCount == 1)
            {
                Fruit fruit = spawnPoint.GetChild(0).GetComponent<Fruit>();
                if (fruit != null)
                {
                    if (fruit.isMultiplier)
                    {
                        Sum *= fruit.GetNumber();
                        formula += $" *{fruit.GetNumber()}";
                    }
                    else
                    {
                        Sum += fruit.GetNumber();

                        if (fruit.GetNumber() >= 0)
                            formula += $" +{fruit.GetNumber()}";
                        else
                            formula += $" {fruit.GetNumber()}";
                    }
                }
                else
                    Debug.LogError("Fruit Component is not defined");
            }
            else
            {
                Debug.Log($"Fruit in {spawnPoint.name} not founded");
            }
        }
        Debug.Log($"Formula {formula}");
        Debug.Log($"Sum {Sum}");
    }
    
    public void MoveFruitsToBasket()
    {
        StartCoroutine(MoveFruitsCoroutine());
    }

    private IEnumerator MoveFruitsCoroutine()
    {
        float moveSpeed = 1f;

        // Создаем массив для хранения целевых позиций корзины для каждого фрукта
        Vector3[] endPositions = new Vector3[spawnedFruits.Count];

        // Заполняем массив случайными позициями из массива точек корзины
        for (int i = 0; i < spawnedFruits.Count; i++)
        {
            RectTransform basketPoint = pointsInBasket[Random.Range(0, pointsInBasket.Length)];
            endPositions[i] = basketPoint.position;

            // Назначаем точку корзины родителем для соответствующего фрукта
            spawnedFruits[i].transform.SetParent(basketPoint);
        }

        float startTime = Time.time;

        while (Time.time - startTime <= 1f) // Вы можете использовать любое время в зависимости от вашего желаемого времени перемещения
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float journeyFraction = distanceCovered / 1f; // Длительность перемещения 1 секунда

            // Перемещаем каждый фрукт к его целевой позиции
            for (int i = 0; i < spawnedFruits.Count; i++)
            {
                Vector3 startPosition = spawnedFruits[i].transform.position;
                Vector3 endPosition = endPositions[i];

                spawnedFruits[i].transform.position = Vector3.Lerp(startPosition, endPosition, journeyFraction);
            }

            yield return null;
        }

        // После завершения перемещения увеличиваем счетчик и спавним новые фрукты
        IncreaseSpawnCount();
        SpawnFruits();
    }
}
