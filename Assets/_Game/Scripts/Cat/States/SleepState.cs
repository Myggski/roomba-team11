using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepState : IState
{
    private CatAnimationManager _catAnimationManager;
    private Transform _playerTr;
    private Transform _catTr;
    private CatVariables _catVariables;
    

    public SleepState(CatAnimationManager catAnimationManager, Transform playerTr, Transform catTr, CatVariables catVariables)
    {
        _catAnimationManager = catAnimationManager;
        _playerTr = playerTr;
        _catTr = catTr;
        _catVariables = catVariables;
    }
    
    public void Enter()
    {
        if (Vector3.Distance(_playerTr.position, _catTr.position) < _catVariables.wakeUpRangeFromPlayer)
        {
            _catAnimationManager.PlaySleepAnimation();
        }
    }

    public void Tick()
    {
        if (Vector3.Distance(_playerTr.position, _catTr.position) < _catVariables.wakeUpRangeFromPlayer)
        {
            Sounds.PlaySound(Sounds.CreateSoundEvent(_catVariables.MeowSound, _catTr), 0.6f);
            _catVariables.changeToWalkToRandomState?.Call();
        }
    }

    public void Exit()
    {
    }
}
