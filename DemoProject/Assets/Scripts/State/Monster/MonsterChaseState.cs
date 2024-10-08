using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChaseState : MonsterState
{
    public MonsterChaseState(Monster monster)
        : base(monster)
    {

    }

    public override void Enter()
    {
        Debug.Log($"{_monster.name} : Chase");
        _monster.SetAnimation(this);
    }

    public override void Update()
    {
        if (_monster.Target == null)
        {
            _monster.StateMachine.TransitionTo(_monster.StateMachine.MonsterReturnState);
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
