using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSetBase : ScriptableObject {
    public abstract string TaskName { get; }
    public abstract int NumberOfChoreItems { get; }
    public abstract int NumberOfChoreItemsCompleted { get; }
    public abstract List<ChoreItemBase> List();
}