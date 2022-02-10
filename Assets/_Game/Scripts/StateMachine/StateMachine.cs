using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public abstract class StateMachine : MonoBehaviour
{
    private IState _currentState;
    
    protected void ChangeState(IState newState)
    {
        if (_currentState == newState)
        {
            return;
        }
        ChangeStateRoutine(newState);
    }

    private void ChangeStateRoutine(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }
    

    public void Update()
    {
        _currentState?.Tick();
    }
}