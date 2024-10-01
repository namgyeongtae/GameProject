using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static INode;

public class GoblinKnight : Enemy
{
    private BTRunner _BTRunner;

    private Transform _enemyTarget;

    private float _waitAttackTime = 0;
    private float _waitInterval = 2.5f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _BTRunner = new BTRunner(SettingBT());

        _waitAttackTime = 1 / _stat.attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        _BTRunner.Execute();
    }

    private INode SettingBT()
    {
        return new SelectorNode
            (
                new List<INode>()
                {
                    new SequenceNode
                    (
                        new List<INode>()
                        {
                            new ActionNode(DetectTarget),
                            new ActionNode(ChaseTarget),
                            new ActionNode(Attack)
                        }
                    ),
                    new ActionNode(Wander)
                }
            );
    }

    private NodeState DetectTarget()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("There is no Player!!");
            return NodeState.Failure;
        }

        if (Vector2.Distance(transform.position, player.transform.position) <= _stat.detectRange)
        {
            Debug.Log("Success Detecting");
            _enemyTarget = player.transform;

            if (!IsArmed) Armed();
            return NodeState.Success;
        }

        _animator.SetBool("IsMove", false);
        _navmeshAgent.isStopped = true;

        _enemyTarget = null;

        Debug.Log("Detect");
        return NodeState.Failure;
    }

    private NodeState ChaseTarget()
    {
        if (_enemyTarget != null )
        {
            if (Vector2.Distance(_enemyTarget.position, transform.position) > _stat.attackRange)
            {
                _navmeshAgent.isStopped = false;
                _navmeshAgent.speed = 1.5f;
                _navmeshAgent.SetDestination(_enemyTarget.position);

                bool flip = _enemyTarget.transform.position.x >= transform.position.x ? true : false;
                FlipSprite(flip);

                _animator.SetBool("IsMove", true);

                _isAttacking = false;

                return NodeState.Running;
            }

            _navmeshAgent.isStopped = true;
            _navmeshAgent.speed = 0f;

            if (!_isAttacking)
                _waitAttackTime = _waitInterval / _stat.attackSpeed;

            return NodeState.Success;
        }

        Debug.Log("Fail Chase");
        return NodeState.Failure;
    }

    private NodeState Attack()
    {
        _isAttacking = true;

        if (_waitAttackTime < _waitInterval / _stat.attackSpeed)
        {
            _waitAttackTime += Time.deltaTime;
            return NodeState.Running;
        }
        else
        {
            // 공격 실시
            _navmeshAgent.isStopped = true;
            _navmeshAgent.speed = 0f;

            _animator.SetTrigger("IsAttack");
            _animator.SetBool("IsMove", false);
            _weapon.Attack();

            _waitAttackTime = 0;
        }

        return NodeState.Success; 
    }
}
