public class RoachObjectPool : ChoreItemObjectPoolBase<RoachChoreItem> {
    protected override void Awake() {
        base.Awake();
        
        SetupPool();
    }
}