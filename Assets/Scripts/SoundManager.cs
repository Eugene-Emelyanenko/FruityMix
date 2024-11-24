using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float musicVolume = 0.25f;
    public float sfxVolume = 0.5f;
    public static SoundManager Instance { get; private set; }

    public AudioSource sfxSource; // Для звуковых эффектов
    public AudioSource musicSource; // Для фоновой музыки

    // Примеры звуковых эффектов
    public AudioClip collectSound;
    public AudioClip mistakeSound;

    // Пример фоновой музыки
    public AudioClip backgroundMusic;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }
    
    private void PlayBackgroundMusic()
    {
        musicSource.volume = musicVolume;
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
    
    public void PlayCollectSound()
    {
        sfxSource.volume = sfxVolume;
        sfxSource.PlayOneShot(collectSound);
    }

    public void PlayMistakeCoinSound()
    {
        sfxSource.volume = sfxVolume;
        sfxSource.PlayOneShot(mistakeSound);
    }
}