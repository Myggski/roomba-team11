using UnityEngine;

public class PawPrintChoreItem : SuckableBase {
    [SerializeField]
    private PawPrintChoreItemSet pawPrintChoreItemSet;

    protected override void Awake() {
        base.Awake();

        pawPrintChoreItemSet.Add(this);
    }

    public override void Release() {
        pawPrintChoreItemSet.SetToComplete(InstanceId);
        PawPrintObjectPool.Instance.Release(this);
        onChoreItemCompleted?.Call(pawPrintChoreItemSet);
    }

    private void OnDestroy() => pawPrintChoreItemSet.Remove(InstanceId);
}