
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [Header("Results Panel")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI fruitsCollectedText;
    [SerializeField] private TextMeshProUGUI mistakesText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private CollectSlider collectSlider;
    
    [Header("Variables")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Colors")]
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color endTimeColor;

    private float timeLeft;
    private bool isTimerRunning = false;

    void Start()
    {
        resultsPanel.SetActive(false);
        timerText.color = defaultColor;
        timeLeft = PlayerPrefs.GetInt("Time", 60);
        UpdateTimerDisplay();
        StartTimer();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timeLeft -= Time.deltaTime;
            
            UpdateTimerDisplay();
            
            if (timeLeft <= 0)
            {
                StopTimer();
            }
        }
    }
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    
    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        timeLeft = 0;
        timerText.color = endTimeColor;
        UpdateTimerDisplay();
        GameOver();
    }

    private void GameOver()
    {
        resultsPanel.SetActive(true);
        timeText.text = ConvertTime(PlayerPrefs.GetInt("Time", 60));
        scoreText.text = collectSlider.score.ToString();
        fruitsCollectedText.text = collectSlider.fruitsCollected.ToString();
        mistakesText.text = collectSlider.mistakes.ToString();
    }
    
    private string ConvertTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void IncreaseTime(int seconds)
    {
        timeLeft += seconds;
    }
}
