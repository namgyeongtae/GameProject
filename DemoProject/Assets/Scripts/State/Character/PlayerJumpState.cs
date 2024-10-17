using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player)
        : base(player)
    {
        
    }

    public override void Enter()
    {
        _player.SetAnimation(this);
    }

    public override void Update()
    {
        if (_player.Rigidbody.velocity.y < 0)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.PlayerFallState);
        }
    }

    public override void Exit() 
    {

    }
}
