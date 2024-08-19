using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdleState : MonsterState
{
    public MonsterIdleState(Monster monster) 
        : base(monster)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{_monster.name} : Idle");
        _monster.SetAnimation(this);
    }

    public override void Update()
    {
        if (_monster.Target != null)
        {
            _monster.StateMachine.TransitionTo(_monster.StateMachine.MonsterChaseState);
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
