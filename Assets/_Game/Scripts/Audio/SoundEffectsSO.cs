using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class SoundEffectsSO : SoundEffects
{
    #region config

    public AudioClip clip;
    [SerializeField, Range(0.01f, 1f)] 
    private float _volume = 1;
    public float Volume => _volume;

    [SerializeField, Range(0.01f, 1f)] 
    private float _pitch = 1;
    public float Pitch => _pitch;

    public void PlayEffect()
    {
        Play(this);
    }

    #endregion

}
