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

        if (_monster.IsReturnToOrigin())
        {
            _monster.StateMachine.TransitionTo(_monster.StateMachine.MonsterIdleState);
            return;
        }

        if (_monster.HP <= 0)
        {
            _monster.StateMachine.TransitionTo(_monster.StateMachine.MonsterDieState);
            return;
        }
    }

    public override void Exit()
    {

    }
}
