using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Mob : MonoBehaviour
{
    [SerializeField] private Stat _monsterStat;
    
    private const float DETECT_RANGE = 5.0f;

    private BTRunner _BTRunner;

    private float _currentHP;
    private float _maxHP;

    private float _currentWaitTime;
    private float _maxWaitTime;

    private Transform _targetPlayer;
    private Vector2 _originPos;

    private NavMeshAgent _navmeshAgent;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    private void Awake()
    {
        _navmeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _BTRunner = new BTRunner(SettingBT());

        _originPos = transform.position;

        _navmeshAgent.updateRotation = false;
        _navmeshAgent.updateUpAxis = false;

        _maxHP = _currentHP = _monsterStat.hp;
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
                        "Detect or Move",
                        new List<INode>()
                        {
                            new ActionNode(DetectTarget),
                            new ActionNode(MoveToPlayer)
                        }
                    )
                     // new ActionNode(ReturnOriginPos)
                }
            );
    }

    #region Action Event
    private INode.NodeState DetectTarget()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("There is no Player!!");
            return INode.NodeState.Failure;
        }

        if (Vector2.Distance(transform.position, player.transform.position) <= DETECT_RANGE)
        {
            _targetPlayer = player.transform;
            return INode.NodeState.Success;
        }

        _animator.SetBool("IsMove", false);
        _navmeshAgent.isStopped = true;

        _targetPlayer = null;

        return INode.NodeState.Running;
    }

    private INode.NodeState MoveToPlayer()
    {
        if (_targetPlayer != null)
        {
            if (Vector2.Distance(_targetPlayer.transform.position, transform.position) <= 0.01f)
                return INode.NodeState.Success;

            _navmeshAgent.isStopped = false;
            _navmeshAgent.speed = 1.5f;
            _navmeshAgent.SetDestination(_targetPlayer.transform.position);

            if (_targetPlayer.transform.position.x >= transform.position.x)
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            else
                transform.rotation = Quaternion.identity;

            _animator.SetBool("IsMove", true);

            return INode.NodeState.Running;
        }

        return INode.NodeState.Failure;
    }

    private INode.NodeState ReturnOriginPos()
    {
        if (_originPos.x >= transform.position.x)
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        else
            transform.rotation = Quaternion.identity;

        if (Vector2.Distance(_originPos, transform.position) < Mathf.Pow(float.Epsilon, 2))
            return INode.NodeState.Success;
        else
        {
            _navmeshAgent.isStopped = false;
            _navmeshAgent.speed = 1.5f;
            _navmeshAgent.SetDestination(_originPos);

            _animator.SetBool("IsMove", true);

            return INode.NodeState.Running;
        }
    }

    private INode.NodeState IsDead()
    {
        if (_currentHP <= 0)
            return INode.NodeState.Success;

        return INode.NodeState.Failure;
    }

    private INode.NodeState Die()
    {
        // TODO
        // Play Die Animation
        // Destroy boss GameObject
        _animator.SetBool("IsDead", true);
        return INode.NodeState.Success;
    }
    #endregion

    public virtual void TakeDamage(GameObject hitSource, float damage)
    {
        damage -= _monsterStat.defense;

        _currentHP -= damage;

        if (_currentHP <= 0)
        {
            Managers.Resource.Destroy(gameObject);
            return;
        }

        StartCoroutine(KnockBack(hitSource));
    }

    private IEnumerator KnockBack(GameObject hitSource)
    {
        _navmeshAgent.isStopped = true;

        float knockBackForce = 2.5f;

        Vector2 direction = transform.position - hitSource.transform.position;
        _rigidbody.AddForce(direction.normalized * knockBackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(5f);

        _navmeshAgent.isStopped = false;
    }
}
