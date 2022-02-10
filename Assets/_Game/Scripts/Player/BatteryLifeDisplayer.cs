using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BatteryLifeDisplayer : MonoBehaviour {
    [SerializeField]
    private Batteries battery;

    private Renderer _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;

    /// <summary>
    /// Calls via GameEventListener
    /// </summary>
    public void UpdateVisualBattery() {
        _materialPropertyBlock = new MaterialPropertyBlock();
        _materialPropertyBlock.SetFloat("_CurrentPosition", battery.CurrentEnergy / battery.BatteryCapacity);
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }

    private void Setup() {
        _renderer = GetComponent<Renderer>();

        UpdateVisualBattery();
    }

    private void Awake() => Setup();
}
