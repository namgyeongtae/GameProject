using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController player)
        : base(player)
    {
        
    }

    public override void Enter()
    {
        Debug.Log("Idle");
        _player.SetAnimation(this);
    }

    public override void Update()
    {
        if (Mathf.Abs(_player.Rigidbody.velocity.x) > 0)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.PlayerRunState);
            return;
        }

        if (_player.Rigidbody.velocity.y > 0)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.PlayerJumpState);
            return;
        }
    }

    public override void Exit()
    {

    }
}
