using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState
{
    private PlayerController _player;

    public PlayerRunState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        Debug.Log("Run");
        _player.SetAnimation(this);
    }

    public void Update()
    {
        if (_player.Rigidbody.velocity.x == 0)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.PlayerIdleState);
            return;
        }

        if (_player.Rigidbody.velocity.y > 0)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.PlayerJumpState);
            return;
        }
    }

    public void Exit()
    {

    }
}
