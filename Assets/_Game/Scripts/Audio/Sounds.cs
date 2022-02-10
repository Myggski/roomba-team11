using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public static class Sounds
{
    private static float _effectVolume;
    
    public static FMOD.Studio.EventInstance CreateSoundEvent(string soundInstance, Transform transform)
    {
        FMOD.Studio.EventInstance soundEvent = FMODUnity.RuntimeManager.CreateInstance(soundInstance);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundEvent, transform);
        return soundEvent;
    }

    public static void PlaySound(FMOD.Studio.EventInstance sound, float volume = 1f)
    {
        sound.setVolume(volume * _effectVolume);
        sound.start();
    }

    public static void StopSound(FMOD.Studio.EventInstance sound, STOP_MODE stopMode)
    {
        sound.stop(stopMode);
    }

    public static void ChangeVolume(FMOD.Studio.EventInstance sound, float volume)
    {
        sound.setVolume(volume * _effectVolume);
    }

    public static void ChangeEffectsVolume(float volume)
    {
        _effectVolume = volume;
    }
 
    
}
