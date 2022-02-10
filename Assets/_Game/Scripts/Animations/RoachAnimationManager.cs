using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoachAnimationManager : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
}
