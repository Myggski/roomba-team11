using UnityEngine;
using Random = UnityEngine.Random;

public class RoachSpawner : MonoBehaviour
{
    [SerializeField]
    private int roaches = 1;
    [SerializeField]
    private float spawnRange = 1;
    [SerializeField]
    private float spawnTimer = 30f;

    [Header("Roaming area")] 
    [SerializeField]
    private bool _showWalkAreaBox;
    [SerializeField] 
    private Color _areaIndicatorColor;
    [SerializeField]
    private Vector3 _randomWalkBoxPos;
    [SerializeField]
    private Vector3 _randomWalkBoxPos1;
    
    private float nextSpawn = 0f;
    private float maxSpawns = 3f;
    private float currentSpawn = 0f;
    
    [SerializeField] 
    private Transform _playerTR;
    private void Update()
    {
        if (Time.time >= nextSpawn && currentSpawn < maxSpawns)
        {
            SpawnRoaches(roaches);
            nextSpawn = Time.time + spawnTimer;
            currentSpawn++;
        }
    }

    private void SpawnRoaches(int n)
    {
        transform.localScale = new Vector3(spawnRange, 1, spawnRange);
        Vector3 spawnScale = transform.localScale * 0.5f;
        
        for (var i = 0; i < n; i++)
        {
            Vector3 spawnPos = new Vector3(
                transform.position.x + Random.Range(-spawnScale.x, spawnScale.x),
                0.61f,
                transform.position.z + Random.Range(-spawnScale.z, spawnScale.z)
            );

            RoachChoreItem roach = RoachObjectPool.Instance.Get();
            roach.gameObject.GetComponent<RoachMovement>().SetWalkPosBox(_randomWalkBoxPos, _randomWalkBoxPos1, _playerTR);
            roach.transform.position = spawnPos;
            roach.transform.rotation = Quaternion.identity;
            roach.gameObject.SetActive(true);
        }
    }
    
    private void OnDrawGizmos()
    {
        if (!_showWalkAreaBox)
            return;
        

        Vector3 pos = _randomWalkBoxPos;
        Vector3 pos1 = _randomWalkBoxPos1;

        Gizmos.color = _areaIndicatorColor;
        Gizmos.DrawSphere(pos, 0.1f);
        Gizmos.DrawLine(new Vector3(pos.x, pos.y, pos.z), new Vector3(pos.x, pos.y+10, pos.z));
        
        Gizmos.DrawSphere(new Vector3(pos.x, pos.y, pos1.z), 0.1f);
        Gizmos.DrawLine(new Vector3(pos.x, pos.y, pos1.z), new Vector3(pos.x, pos.y+10, pos1.z));
        
        Gizmos.DrawSphere(pos1, 0.1f);
        Gizmos.DrawLine(new Vector3(pos1.x, pos1.y, pos1.z), new Vector3(pos1.x, pos1.y+10, pos1.z));
        
        Gizmos.DrawSphere(new Vector3(pos1.x, pos1.y, pos.z), 0.1f);
        Gizmos.DrawLine(new Vector3(pos1.x, pos1.y, pos.z), new Vector3(pos1.x, pos1.y+10, pos.z));
    }
    
}
