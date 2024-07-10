using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : IState
{
    private PlayerController _player;

    public PlayerFallState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        _player.SetAnimation(this);
    }

    public void Update()
    {
        if (_player.IsGrounded())
        {
            Debug.Log("Landing!!");
            if (Mathf.Abs(_player.Rigidbody.velocity.x) > 0)
                _player.StateMachine.TransitionTo(_player.StateMachine.PlayerRunState);
            else
                _player.StateMachine.TransitionTo(_player.StateMachine.PlayerIdleState);
        }
    }

    public void Exit()
    {

    }
}
