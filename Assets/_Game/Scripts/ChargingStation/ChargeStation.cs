using System;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Timeline;

public class ChargeStation : MonoBehaviour
{

    [SerializeField] 
    private GameEvent _enterCharge;
    [SerializeField] 
    private GameEvent _leaveCharge;
    
    [SerializeField]
    private Batteries _batteryToCharge;
    [SerializeField, Range(1f,100f)]
    [Tooltip("The amount of charge each tick")]
    private float _chargePower;

    [SerializeField, Tooltip("In Seconds")]
    private float _chargingDelayBetweenTicks;
    private float _nextTick = 0.0f;

    private bool _charge;

    [SerializeField] 
    private string _chargeSound;
    FMOD.Studio.EventInstance _chargeSoundEvent;

    private void Start()
    {
        _chargeSoundEvent = Sounds.CreateSoundEvent(_chargeSound, transform);
    }

    private void FixedUpdate()
    {
        if (!_charge || Time.time < _nextTick) return;
        
        var currentEnergy = _batteryToCharge.CurrentEnergy;
        var maxEnergy = _batteryToCharge.BatteryCapacity;

        if (currentEnergy >= maxEnergy)
        {
        }
        else if (currentEnergy + _chargePower >= maxEnergy)
        {
            _batteryToCharge.CurrentEnergy = maxEnergy;
        }
        else
        {
            _batteryToCharge.CurrentEnergy += _chargePower; 
        }
        
        _nextTick = Time.time + _chargingDelayBetweenTicks;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        _charge = true;
        _enterCharge?.Call();
        _chargeSoundEvent.start();
        Sounds.PlaySound(_chargeSoundEvent, 1f);
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        _charge = false;
        _leaveCharge?.Call();
        Sounds.StopSound(_chargeSoundEvent, STOP_MODE.ALLOWFADEOUT);
    }
}
