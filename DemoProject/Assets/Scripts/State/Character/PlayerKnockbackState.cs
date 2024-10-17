using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockbackState : PlayerState
{
    public PlayerKnockbackState(Player player)
        : base(player)
    {

    }

    public override void Enter()
    {
        _player.SetAnimation(this);
    }

    public override void Update()
    {
        if (_player.Rigidbody.velocity.magnitude <= 0.7f)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.PlayerIdleState);
            return;
        }
    }

    public override void Exit()
    {

    }
}
