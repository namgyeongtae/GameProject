using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static INode;

public class GoblinArcher : Enemy
{
    private BTRunner _BTRunner;

    private float _waitAttackTime = 0;
    private float _waitInterval = 2.5f;
    private bool _isAttackInProgress = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _BTRunner = new BTRunner(SettingBT());
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
            _waitAttackTime = _waitInterval / _stat.attackSpeed;
            return NodeState.Failure;
        }

        if (Vector2.Distance(transform.position, player.transform.position) <= _stat.detectRange)
        {
            _enemyTarget = player.transform;

            return NodeState.Success;
        }

        _animator.SetBool("IsMove", false);

        _enemyTarget = null;

        return NodeState.Failure;
    }

    private NodeState ChaseTarget()
    {
        if (_enemyTarget != null)
        {
            if (Vector2.Distance(_enemyTarget.position, transform.position) > _stat.attackRange)
            {
                Vector3 dir = _enemyTarget.position - transform.position;

                transform.position += dir * 0.7f * Time.deltaTime;

                bool flip = _enemyTarget.transform.position.x >= transform.position.x ? true : false;
                FlipSprite(flip);

                _animator.SetBool("IsMove", true);

                _isAttacking = false;

                return NodeState.Running;
            }

            _animator.SetBool("IsMove", false);

            return NodeState.Success;
        }

        return NodeState.Failure;
    }

    private NodeState Attack()
    {
        // 공격 중인지 확인
        if (_isAttackInProgress)
        {
            return NodeState.Running; // 공격이 진행 중이라면 Running 상태를 반환
        }

        float interval = _waitInterval / _stat.attackSpeed;

        if (!_isAttacking)
        {
            _isAttacking = true;
            _waitAttackTime = interval / 1.2f; // 대기 시간 초기화
        }

        // 대기 시간이 부족할 경우 시간 누적
        _waitAttackTime += Time.deltaTime;

        if (_waitAttackTime < _waitInterval / _stat.attackSpeed)
        {
            return NodeState.Running;
        }

        // 비동기 공격 시작
        _isAttackInProgress = true;
        PerformAttackAsync(); // 비동기 메서드 호출
        return NodeState.Running;
    }

    private async void PerformAttackAsync()
    {
        // 공격 애니메이션 및 비동기 공격 수행
        _animator.SetTrigger("IsAttack");
        _animator.SetBool("IsMove", false);

        // 비동기 공격 메서드를 기다림
        var bow = (BowWeapon) _weapon;
        await bow.AttackAsync();

        // 공격 후 상태 초기화
        _waitAttackTime = 0;
        _isAttacking = false;
        _isAttackInProgress = false;
    }
}
