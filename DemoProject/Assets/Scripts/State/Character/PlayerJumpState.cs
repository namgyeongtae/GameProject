using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : IState
{
    private PlayerController _player;

    public PlayerJumpState(PlayerController player)
    {
        _player = player;
    }

    public void Enter()
    {
        Debug.Log($"Jump : {_player.Rigidbody.velocity.y}");
        _player.SetAnimation(this);
    }

    public void Update()
    {
        if (_player.Rigidbody.velocity.y < 0)
        {
            _player.StateMachine.TransitionTo(_player.StateMachine.PlayerFallState);
        }
    }

    public void Exit() 
    {

    }
}
