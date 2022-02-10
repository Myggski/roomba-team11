using UnityEngine;
using UnityEngine.AI;
public class Cat : StateMachine
{
    [SerializeField] 
    private CatVariables _catVariables;
    [SerializeField] 
    private Transform _playerTr;
    private Transform _catTr;
    
    private NavMeshAgent _navMeshAgent;
    private WalkToRandomState _walkToRandomState;
    private SleepState _sleepState;
    private CatAnimationManager _catAnimationManager;
    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _catAnimationManager = GetComponentInChildren<CatAnimationManager>();
        _catTr = transform;
        SetupStates();
    }

    private void Start()
    {
        ChangeState(_walkToRandomState);
    }

    private void SetupStates()
    {
        _walkToRandomState = new WalkToRandomState(_navMeshAgent, _catVariables, _playerTr, _catTr, _catAnimationManager);
        _sleepState = new SleepState(_catAnimationManager, _playerTr, _catTr, _catVariables);
    }

    public void ChangeToSleepState()
    {
        ChangeState(_sleepState);
    }

    public void StopFootprints()
    {
        _walkToRandomState._makeFootprints = false;
    }

    public void WakeFromSleep()
    {
        ChangeState(_walkToRandomState);
    }
    
    private void OnDrawGizmos()
    {
        if (!_catVariables._showWalkAreaBox)
            return;

        Vector3 pos = _catVariables.RandomWalkBoxPos;
        Vector3 pos1 = _catVariables.RandomWalkBoxPos1;
        
        Gizmos.color = Color.red;
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
