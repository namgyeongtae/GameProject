using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : IState
{
    protected PlayerController _player;

    public PlayerState(PlayerController player)
    {
        _player = player;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit() 
    {

    }

    public virtual void Update()
    {

    }
}
