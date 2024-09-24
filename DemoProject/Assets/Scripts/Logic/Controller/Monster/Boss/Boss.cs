using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Boss : MonoBehaviour
{
    private const float MAX_DETECT_RANGE = 5.0f;
    private const float MIN_DETECT_RANGE = 1.0f;
    private const float MIN_DASH_RANGE = 10.0f;
    private const float MAX_DASH_RANGE = 15.0f;

    private BTRunner _BTRunner;

    [SerializeField] private Stat _bossStat;
    [SerializeField] private GameObject _unitRoot;

    private int _currentPhase = 1;
    private bool _isExecutePhase = true;

    private float _currentHp;
    private float _maxHp;

    private INode _rootNode;

    private Transform _enemyPlayerTr;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody2D _rigidbody;
    private Animator _animator;

    // Dash After Image
    private float _afterImageDelay = 0.02f;
    private float _afterImageDelayTime;
    private bool _isAfterImage;
    private float _afterImageTime = 0.4f;
    private float _afterImageTimeLeft = 0.4f;

    private bool _isDash;
    private float _dashTimeLeft;
    private Vector2 _dashDirection;
    private float _dashDistance;
    private float _dashTime = 0.5f;
    private float _lastDashTime = -Mathf.Infinity;
    private float _dashCoolDown = 2f;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _maxHp = _currentHp = _bossStat.hp;

        _enemyPlayerTr = GameObject.FindGameObjectWithTag("Player").transform;

        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        BuildBT();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isExecutePhase)
            _BTRunner.Execute();

        UpdateRotation();
        MakeAfterImage();
    }

    void BuildBT()
    {
        switch(_currentPhase)
        {
            case 1:
                _BTRunner = new BTRunner(Phase1Tree());
                break;
            case 2:
                _BTRunner = new BTRunner(Phase2Tree());
                break;
            case 3:
                _BTRunner = new BTRunner(Phase3Tree());
                break;
        }
    }

    INode Phase1Tree()
    {
        return new SequenceNode("Phase 1", new List<INode>()
        {
            new ActionNode(ChasePlayer),
            new ActionNode(RandomDash)
        });
    }

    INode Phase2Tree()
    {
        return new SelectorNode(new List<INode>()
        {
            new SequenceNode("Phase 2", new List<INode>()
            {
                new ActionNode(ChasePlayer),
                new ActionNode(RandomDash)
            }),
            new ActionNode(CreateSwords)
        });
    }

    INode Phase3Tree()
    {
        return new SelectorNode(new List<INode>()
        {
            new ActionNode(ChasePlayer),
            new ActionNode(RandomDash),
            new ActionNode(SummonMonster),
            new ActionNode(GroundAttack),
            new ActionNode(TeleportToPlayer) 
        });
    }

    private void CheckPhase()
    {
        float hpRatio = _currentHp / _maxHp;

        if (hpRatio > 0.7 && _currentPhase != 1)
        {
            _currentPhase = 1;
            BuildBT();
        }
        else if (hpRatio <= 0.7 && hpRatio > 0.4 && _currentPhase != 2)
        {
            Debug.Log("Enter Phase 2");
            _currentPhase = 2;
            BuildBT();
        }
        else if (hpRatio <= 0.4 && _currentPhase != 3) 
        {
            Debug.Log("Enter Phase 3");
            _currentPhase = 3;
            BuildBT();
        }
    }

    #region Phase1 Pattern

    private INode.NodeState ChasePlayer()
    {
        Debug.Log("Chase Player");

        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = 1f;
        _navMeshAgent.SetDestination(_enemyPlayerTr.position);

        _animator.SetBool("IsMove", true);

        if (Vector2.Distance(transform.position, _enemyPlayerTr.position) <= MAX_DETECT_RANGE && 
            Vector2.Distance(transform.position, _enemyPlayerTr.position) >= MIN_DETECT_RANGE)
        {
            return INode.NodeState.Success;
        }

        return INode.NodeState.Running;
    }

    private INode.NodeState RandomDash()
    {
        if (Dash())
        {
            Debug.Log("Dash!");
            return INode.NodeState.Success;
        }
        else
        {
            Debug.Log("Dash Fail");
            return INode.NodeState.Failure;
        }
    }

    #endregion

    #region Phase2 Pattern

    private INode.NodeState CreateSwords()
    {
        if (GetComponentInChildren<SwordHandler>() != null)
            return INode.NodeState.Failure;

        var swordHandler = Managers.Resource.Instantiate("Monster/Boss/SwordHandler", 0, this.transform);

        if (swordHandler == null)
            return INode.NodeState.Failure;

        return INode.NodeState.Success;
    }
    #endregion

    #region Phase3 Pattern

    private INode.NodeState SummonMonster()
    {
        return INode.NodeState.Failure;
    }

    private INode.NodeState GroundAttack()
    {
        return INode.NodeState.Failure;
    }

    private INode.NodeState TeleportToPlayer()
    {
        return INode.NodeState.Failure;
    }

    #endregion

    public void TakeDamage(float damage)
    {
        _currentHp -= damage;

        CheckPhase();

        Debug.Log($"HP : <color=red>{_currentHp}</color>");
    }

    private bool Dash()
    {
        Vector2 dashDirection = (_enemyPlayerTr.position - transform.position).normalized;
        float dashDistance = (_enemyPlayerTr.position - transform.position).magnitude;

        if (_isDash)
        {
            if (_dashTimeLeft > 0)
            {
                _navMeshAgent.isStopped = true;

                // _rigidbody.velocity = dashDirection * (dashDistance / _dashTime);
                _rigidbody.AddForce(dashDirection * (dashDistance / _dashTime));
                _dashTimeLeft -= Time.deltaTime;
                return true;
            }
            else
            {
                _isDash = false;
                _rigidbody.velocity = Vector2.zero;
                return false;
            }

            
        }
        else
        {
            if (Time.time >= (_lastDashTime + _dashCoolDown))
            {
                // StartDash();
                _isDash = true;
                _afterImageTimeLeft = 0f;
                _dashTimeLeft = _dashTime;
                _lastDashTime = Time.time;
            }

            return false;
        }
    }

    private void UpdateRotation()
    {
        Vector2 targetPos = _enemyPlayerTr.position;
        Vector2 targetDir = targetPos - (Vector2)transform.position;

        float dirX = targetDir.x;

        if (dirX < 0)
        {
            _unitRoot.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            _unitRoot.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
    }

    private void MakeAfterImage()
    {
        if (_afterImageTimeLeft >= _afterImageTime)
        {
            return;
        }

        _afterImageTimeLeft += Time.deltaTime;

        if (_afterImageDelayTime > 0)
            _afterImageDelayTime -= Time.deltaTime;
        else
        {
            GameObject afterimage = Managers.Resource.Instantiate("Monster/Boss/BossAfterImage", Define.AFTER_IMAGE_POOL_COUNT);
            SpriteRenderer[] afterImageRenderer = afterimage.GetComponentsInChildren<SpriteRenderer>();
            
            afterimage.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            afterimage.transform.rotation = transform.rotation;
            
            SpriteRenderer[] currentSprite = _unitRoot.GetComponentsInChildren<SpriteRenderer>();
            
            afterimage.transform.localScale = transform.localScale;

            for (int i = 0; i < currentSprite.Length; i++)
            {
                afterImageRenderer[i].sprite = currentSprite[i].sprite;
            }

            _afterImageDelayTime = _afterImageDelay;

            foreach (var sprite in afterImageRenderer)
            {
                sprite.DOFade(0.2f, 0.3f);
            }

            StartCoroutine(Managers.Resource.Destroy(afterimage, 0.3f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            player.TakeDamage(this.gameObject, _bossStat.attack);
        }
    }
}
