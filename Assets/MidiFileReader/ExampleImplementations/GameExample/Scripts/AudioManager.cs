using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource musicSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
        PlayMusic();
        MasterClock.instance.StartCounter();
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }


	
}
