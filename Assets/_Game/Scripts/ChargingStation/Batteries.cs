using UnityEngine;

[CreateAssetMenu(fileName = "New Roomba Battery", menuName = "Batteries/Roomba Battery")]
public class Batteries : ScriptableObject
{
    [SerializeField] 
    private float _batteryCapacity;

    [SerializeField] 
    private float _currentEnergy;
    
    [SerializeField]
    [Range(0, 100)]
    [Tooltip("How much percent is considered to be low energy.")]
    private float _lowBatteryThreshold;

    [SerializeField]
    [Tooltip("Triggers everytime the value of the battery capacity has changed.")]
    private GameEvent onBatteryChanged;
    
    [SerializeField]
    [Tooltip("Triggers once when the battery capacity has changed under or equal to the low-battery threshold.")]
    private GameEvent onBatteryLowEnergy;
    
    [SerializeField]
    [Tooltip("Triggers once when the battery capacity has changed over the low-battery threshold.")]
    private GameEvent onBatteryRecharged;

    private bool _hasTriggeredLowBattery;

    public float BatteryCapacity => _batteryCapacity;

    public bool HasLowBattery => _currentEnergy / _batteryCapacity <= _lowBatteryThreshold / 100f;
    
    // TEST TO GET FULL CHARGE EVERY TIME YOU RESTART
    // Robert
    private void OnEnable() 
    {
        _currentEnergy = 100f;
    }
    
    public float CurrentEnergy
    {
        get => _currentEnergy;
        set {
            _currentEnergy = value;

            if (HasLowBattery && !_hasTriggeredLowBattery) {
                _hasTriggeredLowBattery = true;
                onBatteryLowEnergy?.Call();
            } else if (!HasLowBattery && _hasTriggeredLowBattery) {
                _hasTriggeredLowBattery = false;
                onBatteryRecharged?.Call();
            }
            
            onBatteryChanged?.Call();
        }
    }
}
