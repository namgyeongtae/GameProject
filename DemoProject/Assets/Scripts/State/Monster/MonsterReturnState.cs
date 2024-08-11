using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterReturnState : MonsterState
{
    public MonsterReturnState(Monster monster)
        : base(monster)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{_monster.name} : Return");
        _monster.SetAnimation(this);
    }

    public override void Update()
    {
        if (_monster.Target != null)
        {
            _monster.StateMachine.TransitionTo(_monster.StateMachine.MonsterChaseState);
            return;
        }

    }

    public override void Exit()
    {

    }
}
