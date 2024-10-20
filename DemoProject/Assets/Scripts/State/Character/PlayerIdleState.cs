using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player player)
        : base(player)
    {
        
    }

    public override void Enter()
    {
        _player.SetAnimation(this);
    }

    public override void Update()
    {
        Vector2 velocity = _player.Rigidbody.velocity;

        if (velocity.magnitude > 0.25f)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.PlayerRunState);
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
