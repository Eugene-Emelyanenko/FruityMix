using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectSlider : MonoBehaviour, IEndDragHandler
{
    [SerializeField] private Timer timer;
    
    [SerializeField] private Drum[] drums;
    [SerializeField] private FruitSpawner fruitSpawner;
    public float slideThreshold = 0.85f;
    
    [SerializeField] Image bgImage;
    public float pulseDuration = 1f;
    public float returnDuration = 0.2f;
    public float pulseIntensity = 1.5f;
    private Color originalColor;
    private Coroutine pulseCoroutine;
    
    private Slider slider;
    private float startPosX;
    private float trackWidth;
    private int number = 0;
    public int score = 0;
    public int mistakes = 0;
    public int fruitsCollected = 0;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Start()
    {
        fruitsCollected = 0;
        mistakes = 0;
        score = 0;
        trackWidth = slider.GetComponent<RectTransform>().rect.width;
        originalColor = bgImage.color;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float handlePosX = slider.handleRect.localPosition.x;
        float slidePercentage = (handlePosX + trackWidth * 0.5f) / trackWidth;
        
        if (slidePercentage >= slideThreshold)
        {
            // Выполняется ваш метод, так как пользователь довел handle до конца
            Collect();
            slider.value = slider.minValue;
        }
        else
        {
            // Возвращение handle в начало, так как пользователь не довел его до конца
            slider.value = slider.minValue;
        }
    }

    private void Collect()
    {
        string combinedString = drums[0].SelectedNumber.ToString() + drums[1].SelectedNumber.ToString();
        number = int.Parse(combinedString);

        if (fruitSpawner.Sum == number)
        {
            SoundManager.Instance.PlayCollectSound();
            timer.IncreaseTime(10);
            fruitsCollected += fruitSpawner.spawnCount;
            score += fruitSpawner.Sum;
            fruitSpawner.MoveFruitsToBasket();
        }
        else
        {
            SoundManager.Instance.PlayMistakeCoinSound();
            mistakes++;
            StartPulse();
        }
    }
    
    private void StartPulse()
    {
        if (pulseCoroutine != null)
        {
            StopCoroutine(pulseCoroutine);
        }
        pulseCoroutine = StartCoroutine(Pulse());
    }
    
    IEnumerator Pulse()
    {
        float elapsedTime = 0f;
        while (elapsedTime < pulseDuration)
        {
            float t = Mathf.PingPong(elapsedTime * pulseIntensity, 1f);
            Color newColor = Color.Lerp(originalColor, Color.red, t);
            bgImage.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        float returnTime = 0f;
        while (returnTime < returnDuration)
        {
            float t = returnTime / returnDuration;
            Color newColor = Color.Lerp(bgImage.color, originalColor, t);
            bgImage.color = newColor;
            returnTime += Time.deltaTime;
            yield return null;
        }

        bgImage.color = originalColor;
    }
}
