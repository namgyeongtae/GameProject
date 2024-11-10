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
        // ���� ������ Ȯ��
        if (_isAttackInProgress)
        {
            return NodeState.Running; // ������ ���� ���̶�� Running ���¸� ��ȯ
        }

        float interval = _waitInterval / _stat.attackSpeed;

        if (!_isAttacking)
        {
            _isAttacking = true;
            _waitAttackTime = interval / 1.2f; // ��� �ð� �ʱ�ȭ
        }

        // ��� �ð��� ������ ��� �ð� ����
        _waitAttackTime += Time.deltaTime;

        if (_waitAttackTime < _waitInterval / _stat.attackSpeed)
        {
            return NodeState.Running;
        }

        // �񵿱� ���� ����
        _isAttackInProgress = true;
        PerformAttackAsync(); // �񵿱� �޼��� ȣ��
        return NodeState.Running;
    }

    private async void PerformAttackAsync()
    {
        // ���� �ִϸ��̼� �� �񵿱� ���� ����
        _animator.SetTrigger("IsAttack");
        _animator.SetBool("IsMove", false);

        // �񵿱� ���� �޼��带 ��ٸ�
        var bow = (BowWeapon) _weapon;
        await bow.AttackAsync();

        // ���� �� ���� �ʱ�ȭ
        _waitAttackTime = 0;
        _isAttacking = false;
        _isAttackInProgress = false;
    }
}
