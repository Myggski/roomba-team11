using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IState
{
    private NavMeshAgent _navMeshAgent;
    private GameObject _chaseGameObject;
    public ChaseState(NavMeshAgent navMeshAgent, GameObject chaseGameObject)
    {
        _navMeshAgent = navMeshAgent;
        _chaseGameObject = chaseGameObject;
    }
    
    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Tick()
    {
        _navMeshAgent.destination = _chaseGameObject.transform.position;
    }

    public void Exit()
    { 
        throw new System.NotImplementedException();
    }
}
