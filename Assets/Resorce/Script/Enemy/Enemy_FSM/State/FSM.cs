using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    public FSM(BaseState initState)
    {
        _currentState = initState;

    }

    private BaseState _currentState;

    public void ChangeState(BaseState nextState)
    {
        if (nextState == _currentState)
            return;
        if (_currentState != null)
        {
            _currentState.OnStateExit();
        }
        _currentState = nextState;
        _currentState.OnStateEnter();
    }

    public void UpdateState()
    {
        if (_currentState != null)
        {
            _currentState.OnStateUpdate();
        }
    }
}
