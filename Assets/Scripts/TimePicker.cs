using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimePicker : MonoBehaviour
{
    [SerializeField] private GameObject startGamePanel;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI chooseTimeButtonText;
    private int time = 0;

    private void OnEnable()
    {
        Reset();
    }
    
    public void ChoseTime(int value)
    {
        time = value;
        PlayerPrefs.SetInt("Time", time);
        timeText.text = ConvertTime(time);
        chooseTimeButtonText.text = ConvertTime(time);
    }
    
    private string ConvertTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void OpenStartGamePanel()
    {
        if(timeText.text == "Choose game time")
            return;
        else
        {
            startGamePanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void Reset()
    {
        timeText.text = "Choose game time";
        time = 0;
        chooseTimeButtonText.text = "00:00";
    }
}
