using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuntimeSet<T> : RuntimeSetBase  where T : ChoreItemBase {
    [SerializeField]
    private string taskName;

    protected Dictionary<int, T> _items = new Dictionary<int, T>();
    
    public override string TaskName => taskName;
    public override int NumberOfChoreItems => _items.Count;
    public override int NumberOfChoreItemsCompleted => _items.Count(item => item.Value.ChoreCompleted);
    
    public override List<ChoreItemBase> List() {
        return _items.Values
            .Select(x => x as ChoreItemBase)
            .ToList();
    }

    public bool HasCompletedAllTasks => NumberOfChoreItemsCompleted >= NumberOfChoreItems;
    
    protected virtual void OnEnable()
    {
        _items.Clear();
    }

    public T Get(int instanceId) {
        _items.TryGetValue(instanceId, out T item);

        return item;
    }

    public virtual void Add(T value) {
        _items.Add(value.InstanceId, value);
    }
    
    public void Remove(int instanceId) {
        _items.Remove(instanceId);
    }
    
    public void SetToComplete(int instanceId) {
        T item = Get(instanceId);

        if (ReferenceEquals(item, null)) {
            return;
        }

        item.ChoreCompleted = true;
    }
}