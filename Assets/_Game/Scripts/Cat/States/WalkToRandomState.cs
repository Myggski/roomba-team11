using UnityEngine;
using UnityEngine.AI;

public class WalkToRandomState : IState
{
    private NavMeshAgent _navMeshAgent;
    private Transform _catTr;
    private Transform _playerTr;
    private CatVariables _catVariables;
    private CatAnimationManager _catAnimationManager;
    
    private Vector3 _walkToPos;
    private bool _canWalk;

    private float _maxWalkingTime;
    private float _newWalkPosTime = 0.0f;

    private GameEvent _footstepGameEvent;
    public bool _makeFootprints = true;
    
    private float _makeSoundTime = 11;
    
    public WalkToRandomState(NavMeshAgent navMeshAgent, CatVariables catVariables, Transform playerTr, Transform catTr, CatAnimationManager catAnimationManager)
    {
        _navMeshAgent = navMeshAgent;
        _catVariables = catVariables;
        _playerTr = playerTr;
        _catTr = catTr;
        _catAnimationManager = catAnimationManager;

        RandomWalkPosition(_catVariables.RandomWalkBoxPos, _catVariables.RandomWalkBoxPos1);
        _newWalkPosTime = 0.0f;
    }
    public void Enter()
    {
    }
    public void Tick()
    {
        if (Time.time >= _makeSoundTime)
        {
            float _makeSoundDelay = Random.Range(_catVariables.MeowSoundDelay.x, _catVariables.MeowSoundDelay.y);
            _makeSoundTime = Time.time + _makeSoundDelay;
            Sounds.PlaySound(Sounds.CreateSoundEvent(_catVariables.MeowSound, _catTr), 0.6f);
        }
        
        float currentMoveSpeed = _navMeshAgent.velocity.magnitude;
        float distanceToRoomba = Vector3.Distance(_playerTr.position, _catTr.position);
        float distanceBetweenCatAndPos = Vector3.Distance(_catTr.position, _walkToPos);

        if (distanceToRoomba < _catVariables.wakeUpRangeFromPlayer)
        {
            if (distanceBetweenCatAndPos < 1f)
                RandomWalkPosition(_catVariables.RandomWalkBoxPos, _catVariables.RandomWalkBoxPos1);
            Move();
            _catAnimationManager.PlayWalkingAnimation();
            return;
        }
        
        if (currentMoveSpeed < 0.05f && distanceToRoomba > _catVariables.wakeUpRangeFromPlayer)
            _catAnimationManager.PlaySleepAnimation();
        else
            _catAnimationManager.PlayWalkingAnimation();
        
        if (Vector3.Distance(_catTr.position, _walkToPos) > 0.2f && Time.time < _newWalkPosTime)
        {
            Move();
            if (currentMoveSpeed > 1f && _makeFootprints)
                _catVariables.makeFootstepsEvent?.Call();
            return;
        }
        
        if (!_makeFootprints && _navMeshAgent.velocity.magnitude < 0.05f && distanceToRoomba > _catVariables.wakeUpRangeFromPlayer)
        {
            _catVariables.changeToSleepState?.Call();
        }
        
        RandomWalkPosition(_catVariables.RandomWalkBoxPos, _catVariables.RandomWalkBoxPos1);
    }
    public void Exit()
    {
    }

    private void Move()
    {
        _navMeshAgent.destination = _walkToPos;
    }
    public void RandomWalkPosition(Vector3 pos, Vector3 pos1)
    {
        float xMin = pos.x < pos1.x ? pos.x : pos1.x;
        float xMax = pos.x > pos1.x ? pos.x : pos1.x;
        
        float yMin = pos.y < pos1.y ? pos.y : pos1.y;
        float yMax = pos.y > pos1.y ? pos.y : pos1.y;

        float zMin = pos.z < pos1.z ? pos.z : pos1.z;
        float zMax = pos.z > pos1.z ? pos.z : pos1.z;

        _walkToPos = new Vector3(Random.Range(xMin,xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));
        _maxWalkingTime = Random.Range(_catVariables.delayBetweenWalking.x, _catVariables.delayBetweenWalking.y);
        _newWalkPosTime = Time.time + _maxWalkingTime;
    }
}
