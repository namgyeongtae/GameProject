using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(Player player)
        : base(player)
    {
        
    }

    public override void Enter()
    {
        _player.SetAnimation(this);
    }

    public override void Update()
    {
        //if (_player.IsGrounded())
        //{
        //    Debug.Log("Landing!!");
        //    if (Mathf.Abs(_player.Rigidbody.velocity.x) > 0)
        //        _player.StateMachine.TransitionTo(_player.StateMachine.PlayerRunState);
        //    else
        //        _player.StateMachine.TransitionTo(_player.StateMachine.PlayerIdleState);
        //}
    }

    public override void Exit()
    {

    }
}
