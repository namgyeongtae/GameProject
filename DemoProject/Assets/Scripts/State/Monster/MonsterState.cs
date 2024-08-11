using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState : IState
{
    protected Monster _monster;

    public MonsterState(Monster monster)
    {
        _monster = monster;
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
