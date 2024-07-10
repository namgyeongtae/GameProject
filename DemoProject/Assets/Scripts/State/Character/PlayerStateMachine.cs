using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public IState CurrentState { get; private set; }

    public PlayerIdleState PlayerIdleState { get; private set; }
    public PlayerRunState PlayerRunState { get; private set; }
    public PlayerJumpState PlayerJumpState { get; private set; }
    public PlayerFallState PlayerFallState { get; private set; }

    public PlayerStateMachine(PlayerController player)
    {
        PlayerIdleState = new PlayerIdleState(player);
        PlayerRunState = new PlayerRunState(player);
        PlayerJumpState= new PlayerJumpState(player);
        PlayerFallState= new PlayerFallState(player);
    }

    public void OnInit()
    {
        CurrentState = PlayerIdleState;
        CurrentState.Enter();
    }

    public void OnUpdate()
    {
        CurrentState.Update();
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();

        CurrentState = nextState;
        CurrentState.Enter();
    }
}
