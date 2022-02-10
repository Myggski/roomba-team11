using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class ChoreItemObjectPoolBase<T> : MonoBehaviour where T : ChoreItemBase {
    [SerializeField]
    private int totalItemsToSpawn;
    [SerializeField]
    private List<T> _prefabsToSpawn = new List<T>();
    
    private Queue<T> _objectPool;
    private Vector3 _spawnPosition;
    private static ChoreItemObjectPoolBase<T> _instance;

    public static ChoreItemObjectPoolBase<T> Instance => _instance;

    public int TotalItemsToSpawn => totalItemsToSpawn;

    public T Get() {
        return _objectPool.Count > 0 
            ? _objectPool.Dequeue() 
            : AddItemToPool();
    }
    
    public void Release(T choreItemBase) {
        choreItemBase.gameObject.SetActive(false);
    }

    public void SetupPool(Vector3 spawnPosition = default) {
        _spawnPosition = spawnPosition;

        PreparePool();
    }
    
    public void SetupPrefab(List<T> prefab) {
        _prefabsToSpawn.AddRange(prefab);
    }
    
    private void Setup() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        _objectPool = new Queue<T>();
    }

    private void PreparePool() {
        for (int i = 0; i < totalItemsToSpawn; i++) {
            AddItemToPool();
        }
    }

    private T AddItemToPool() {
        T choreItem = Instantiate();
        Release(choreItem);
        _objectPool.Enqueue(choreItem);

        return choreItem;
    }

    private T GetPrefab() {
        return _prefabsToSpawn[Random.Range(0, _prefabsToSpawn.Count)];
    }

    protected virtual T Instantiate() {
        return Instantiate(GetPrefab(), _spawnPosition, Quaternion.identity);
    }

    protected virtual void Awake() => Setup();
}
