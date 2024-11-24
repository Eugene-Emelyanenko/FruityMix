using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    void Start()
    {
        UnPause();
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }
    
    public void UnPause()
    {
        Time.timeScale = 1;
    }
}
