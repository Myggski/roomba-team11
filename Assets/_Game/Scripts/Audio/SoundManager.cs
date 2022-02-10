using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] 
    private string _backgroundMusic;
    private FMOD.Studio.EventInstance _backgroundMusicEvent;
    [SerializeField]
    private FloatVariable _bgMusicVolume;
    
    [SerializeField]
    private FloatVariable _effectsVolume;
    
    
    private void Awake()
    {
        ChangeEffectsVolume();
        _backgroundMusicEvent = Sounds.CreateSoundEvent(_backgroundMusic, Camera.main.transform);
        _backgroundMusicEvent.setVolume(_bgMusicVolume.Value);
        _backgroundMusicEvent.start();
    }
    public void ChangeMusicVolume()
    {
        _backgroundMusicEvent.setVolume(_bgMusicVolume.Value);
    }

    public void ChangeEffectsVolume()
    {
        Sounds.ChangeEffectsVolume(_effectsVolume.Value);
    }
    
    private void OnDisable()
    {
        Sounds.StopSound(_backgroundMusicEvent, STOP_MODE.IMMEDIATE);
    }
}

