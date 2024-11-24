using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fruit : MonoBehaviour
{
    [SerializeField] private int number = 0;
    public bool isMultiplier = false;
    
    public float fadeInDuration = 1.0f; // Продолжительность появления в секундах

    private Image image;
    void Start()
    {
        image = GetComponent<Image>();
        if (image == null)
            Debug.LogError("Image component is not assigned!");

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // Устанавливаем альфа-канал в ноль, чтобы изображение было невидимым в начале
        Color color = image.color;
        color.a = 0.0f;
        image.color = color;

        float timer = 0.0f;

        // Плавное появление изображения
        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0.0f, 1.0f, timer / fadeInDuration);
            image.color = color;
            yield return null;
        }

        // Убеждаемся, что альфа-канал стал точно равен 1
        color.a = 1.0f;
        image.color = color;
    }

    public int GetNumber() => number;
}
