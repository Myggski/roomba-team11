using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestRoomba : MonoBehaviour
{
    [SerializeField] 
    private GameObject _chargingStation;
    private PowerSource _battery;
    
    private SoundEffects _soundEffects;
    [SerializeField] 
    private SoundEffectsSO roombaCollisionSfx;


    private void Start()
    {
        _battery = GetComponent<PowerSource>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            gameObject.transform.position = _chargingStation.transform.position + Vector3.up;
        }

        if (Input.GetKeyDown("s"))
        {
            _battery.Battery.CurrentEnergy = 0;
        }
        
        if (Input.GetKeyDown("a"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Collision");
        }
        
    }
}
