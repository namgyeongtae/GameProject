using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(PlayerController player)
        : base(player)
    {
        
    }

    public override void Enter()
    {
        Debug.Log("Run");
        _player.SetAnimation(this);
    }

    public override void Update()
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

    public override void Exit()
    {

    }
}
