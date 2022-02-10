using System;
using UnityEngine;

using Random = UnityEngine.Random;


public class ChaseTest : MonoBehaviour
{
    [SerializeField, Tooltip("In Seconds")]
    private float DelayBetweenMoving = 3f;
    private float _nextMove = 0.0f;
    
    private void Update()
    {
        if ( Time.time < _nextMove) return;
        Vector3 newPos = new Vector3(Random.Range(-5f, 5f), transform.position.y, Random.Range(-5f, 5f));
        transform.position = newPos;
        _nextMove = Time.time + DelayBetweenMoving;
    }
}
