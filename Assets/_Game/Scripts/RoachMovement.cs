using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class RoachMovement : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rigidbody;
    private Vector3 originalPosition;
    private Vector3 _destination;
    [SerializeField] 
    private string _moveSound;
    private FMOD.Studio.EventInstance _moveSoundEvent;
    
    private Vector3 _randomWalkBoxPos;
    private Vector3 _randomWalkBoxPos1;
    
    private Transform _playerTR;
    
    private void OnEnable()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        originalPosition = transform.position;
        _destination = NewDestination();
        _moveSoundEvent = Sounds.CreateSoundEvent(_moveSound, transform);
    }

    private void OnDisable()
    {
        FMOD.Studio.PLAYBACK_STATE state;
        _moveSoundEvent.getPlaybackState(out state);
        if (state == PLAYBACK_STATE.PLAYING)
            Sounds.StopSound(_moveSoundEvent, STOP_MODE.IMMEDIATE);
    }

    private float newDestinationDelay = 5f;
    private float nextDestination = 5f;
    private void Update()
    {
        
        if (Vector3.Distance(transform.position, _destination) >= 0.2f && Time.time < nextDestination)
        {
            if (Vector3.Distance(transform.position, _playerTR.position) > 1f)
            {
                _navMeshAgent.destination = _destination;
            }
            
        }
        else
        {
            _destination = NewDestination();
            nextDestination = Time.time + newDestinationDelay;
        }
        
        FMOD.Studio.PLAYBACK_STATE state;
        _moveSoundEvent.getPlaybackState(out state);
        if (state == PLAYBACK_STATE.STOPPED)
        {
            _moveSoundEvent = Sounds.CreateSoundEvent(_moveSound, transform);
            Sounds.PlaySound(_moveSoundEvent, 0.03f);
        }
            
    }

    private Vector3 NewDestination()
    {
        return RandomWalkPosition(_randomWalkBoxPos, _randomWalkBoxPos1);
        //originalPosition + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
    }

    public void SetWalkPosBox(Vector3 pos, Vector3 pos1, Transform playerTr)
    {
        _randomWalkBoxPos = pos;
        _randomWalkBoxPos1 = pos1;
        _playerTR = playerTr;
    }
    private Vector3 RandomWalkPosition(Vector3 pos, Vector3 pos1)
    {
        float xMin = pos.x < pos1.x ? pos.x : pos1.x;
        float xMax = pos.x > pos1.x ? pos.x : pos1.x;
        
        float yMin = pos.y < pos1.y ? pos.y : pos1.y;
        float yMax = pos.y > pos1.y ? pos.y : pos1.y;

        float zMin = pos.z < pos1.z ? pos.z : pos1.z;
        float zMax = pos.z > pos1.z ? pos.z : pos1.z;

        return new Vector3(Random.Range(xMin,xMax), transform.position.y, Random.Range(zMin, zMax));
    }
    
}
