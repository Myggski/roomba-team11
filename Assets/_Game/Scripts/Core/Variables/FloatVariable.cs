using UnityEngine;

[CreateAssetMenu(fileName = "New FloatVariable", menuName = "Variables/FloatVariable")]
public class FloatVariable : ScriptableObject
{
    [SerializeField] 
    private float value;
    [SerializeField]
    private bool rememberValue;

    private float _currentValue;

    public float Value => rememberValue ? value : _currentValue;
    
    public void ChangeValue(float newValue)
    {
        if (rememberValue) {
            value = newValue;
        } else {
            _currentValue = newValue;    
        }
        
    }

    public void ApplyValue(float valueToApply) {
        if (rememberValue) {
            value += valueToApply;
        } else {
            _currentValue += valueToApply;
        }
        
    }

    private void Setup() {
        _currentValue = value;
    }

    private void OnEnable() => Setup();
}