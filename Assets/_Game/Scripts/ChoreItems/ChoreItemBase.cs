using UnityEngine;

public abstract class ChoreItemBase : MonoBehaviour {
    private int _instanceId;
    private bool _choreCompleted;
    public int InstanceId => _instanceId;
    
    public bool ChoreCompleted {
        get => _choreCompleted;
        set => _choreCompleted = value;
    }

    protected virtual void Awake() => _instanceId = GetInstanceID();
}