using UnityEngine;

[CreateAssetMenu(fileName = "New Roomba Battery", menuName = "Batteries/Roomba Battery")]
public class Batteries : ScriptableObject
{
    [SerializeField] 
    private float _batteryCapacity;
    [SerializeField]
    private float _currentEnergy;
}
