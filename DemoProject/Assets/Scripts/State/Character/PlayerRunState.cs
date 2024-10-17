using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(Player player)
        : base(player)
    {
        
    }

    public override void Enter()
    {
        _player.SetAnimation(this);
    }

    public override void Update()
    {
        if (_player.Rigidbody.velocity.magnitude <= 0.25f)
        {
            // Debug.Log("Player : Idle");
            _player.StateMachine.TransitionTo(_player.StateMachine.PlayerIdleState);
            return;
        }

        //if (_player.Rigidbody.velocity.y > 0)
        //{
        //    _player.StateMachine.TransitionTo(_player.StateMachine.PlayerJumpState);
        //    return;
        //}
    }

    public override void Exit()
    {

    }
}
