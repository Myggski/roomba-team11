using UnityEngine;

public class RoachChoreItem : SuckableBase {
    [SerializeField]
    private RoachChoreItemSet roachChoreItemSet;

    protected override void Awake() {
        base.Awake();

        roachChoreItemSet.Add(this);
    }

    public override void Release() {
        roachChoreItemSet.SetToComplete(InstanceId);
        RoachObjectPool.Instance.Release(this);
        onChoreItemCompleted?.Call(roachChoreItemSet);
    }

    private void OnDestroy() => roachChoreItemSet.Remove(InstanceId);
}