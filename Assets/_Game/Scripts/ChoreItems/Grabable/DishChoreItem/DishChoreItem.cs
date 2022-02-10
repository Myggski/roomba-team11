using UnityEngine;

public class DishChoreItem : GrabableChoreItemBase {
    [SerializeField]
    private DishChoreItemSet dishChoreItemSet;
    [SerializeField]
    private GameEventInt OnDishesDropped;

    private bool _isBeingHold;
    
    public override bool IsBeingHold {
        get => _isBeingHold;
        set {
            _isBeingHold = value;

            if (!_isBeingHold) {
                OnDishesDropped.Call(InstanceId);
            }
        }
    }

    protected override void Awake() {
        base.Awake();

        dishChoreItemSet.Add(this);
    }
    private void OnDestroy() => dishChoreItemSet.Remove(InstanceId);
}