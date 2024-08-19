using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDieState : MonsterState
{
    public MonsterDieState(Monster monster) 
        : base(monster)
    {

    }

    public override void Enter()
    {
        Managers.Resource.Destroy(_monster.gameObject);
    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {

    }
}
