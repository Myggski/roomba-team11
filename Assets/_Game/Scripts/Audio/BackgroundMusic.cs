using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] 
    private string _backgroundMusic;
    
 
    
    private float _volume;

    private FMOD.Studio.EventInstance _backgroundMusicEvent;

    private void Awake()
    {
        _backgroundMusicEvent = Sounds.CreateSoundEvent(_backgroundMusic, Camera.main.transform);
        Sounds.PlaySound(_backgroundMusicEvent, _volume);
    }

    private void ChangeBgVolume(float _volume)
    {
        Sounds.ChangeVolume(_backgroundMusicEvent, _volume);
    }

    private void OnDisable()
    {
        Sounds.StopSound(_backgroundMusicEvent, STOP_MODE.IMMEDIATE);
    }
}
