using UnityEngine;

public class LaundryChoreItem : GrabableChoreItemBase {
    [SerializeField]
    private LaundryChoreItemSet laundryChoreItemSet;
    [SerializeField]
    private GameEventInt OnLaundryDropped;

    private bool _isBeingHold;

    public override bool IsBeingHold {
        get => _isBeingHold;
        set {
            _isBeingHold = value;

            if (!_isBeingHold) {
                OnLaundryDropped.Call(InstanceId);
            }
        }
    }

    protected override void Awake() {
        base.Awake();

        laundryChoreItemSet.Add(this);
    }
    private void OnDestroy() => laundryChoreItemSet.Remove(InstanceId);
}