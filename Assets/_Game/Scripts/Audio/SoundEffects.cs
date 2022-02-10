using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoundEffects : ScriptableObject
{
    protected AudioSource Play(SoundEffectsSO clip, AudioSource audioSourceParam = null)
    {
        if (!clip)
        {
            Debug.Log("Missing sound clips");
            return null;
        }

        var source = audioSourceParam;
        if (source == null)
        {
            var _obj = new GameObject("Sound", typeof(AudioSource));
            source = _obj.GetComponent<AudioSource>();
        }

        source.clip = clip.clip;
        source.volume = clip.Volume;
        source.pitch = clip.Pitch;

        source.Play();
        
        Destroy(source.gameObject, source.clip.length / source.pitch);

        return source;
    }
}
