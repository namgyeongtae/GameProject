using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine
{
    public IState CurrentState { get; private set; }

    public MonsterIdleState MonsterIdleState { get; private set; }
    public MonsterChaseState MonsterChaseState { get; private set; }
    public MonsterReturnState MonsterReturnState { get; private set; }
    public MonsterDieState MonsterDieState { get; private set; }

    public MonsterStateMachine(Monster monster)
    {
        MonsterIdleState = new MonsterIdleState(monster);
        MonsterChaseState = new MonsterChaseState(monster);
        MonsterReturnState = new MonsterReturnState(monster);
        MonsterDieState = new MonsterDieState(monster);
    }

    public void OnInit()
    {
        CurrentState = MonsterIdleState;
        CurrentState.Enter();
    }

    public void OnUpdate()
    {
        CurrentState.Update();
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();

        CurrentState = nextState;
        CurrentState.Enter();
    }
}
