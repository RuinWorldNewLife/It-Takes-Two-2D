using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }
    public PlayerState PreviousState { get; private set; }

    public void Initialize(PlayerState startingState)
    {
        PreviousState = startingState;
        CurrentState = startingState;
        CurrentState.Enter();
    }
    public void ChangeState(PlayerState newState)
    {
        PreviousState = CurrentState;
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
