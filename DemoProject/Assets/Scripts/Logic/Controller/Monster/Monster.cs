using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] private Stat _monsterStat;   

    private Rigidbody2D _rigidbody;
    private NavMeshAgent _navmeshAgent;

    private AISensor _aiSensor;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private GameObject _target;
    private Vector3 _originPosition;

    public MonsterStateMachine StateMachine { get; private set; }
    public GameObject Target => _target;

    private void Awake()
    {
        StateMachine = new MonsterStateMachine(this);

        _navmeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _aiSensor = GetComponentInChildren<AISensor>();
        BindAIEvents();

        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StateMachine.OnInit();

        _originPosition = transform.position;

        _navmeshAgent.updateRotation = false;
        _navmeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.OnUpdate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected void BindAIEvents()
    {
        if (_aiSensor == null)
            return;

        _aiSensor.OnTriggerEnterEvent -= DetectTarget;
        _aiSensor.OnTriggerEnterEvent += DetectTarget;
        _aiSensor.OnTriggerExitEvent -= MissTarget;
        _aiSensor.OnTriggerExitEvent += MissTarget;
    }

    protected virtual void Move()
    {
        switch(StateMachine.CurrentState)
        {
            case MonsterIdleState idle:
                _navmeshAgent.isStopped = true;
                return;
            case MonsterChaseState chase:
                Chase();
                break;
            case MonsterReturnState returnState:
                Return();
                break;
        }
    }

    private void Return()
    {
        _navmeshAgent.isStopped = false;
        _navmeshAgent.speed = _monsterStat.speed;
        _navmeshAgent.SetDestination(_originPosition);
    }

    private void Chase()
    {
        _navmeshAgent.isStopped = false;
        _navmeshAgent.speed = _monsterStat.speed;
        _navmeshAgent.SetDestination(_target.transform.position);
    }

    protected virtual void TakeDamage(float damage)
    {

    }

    public void SetAnimation(IState state)
    {
        switch (state)
        {
            case MonsterIdleState idle:
                _animator.SetBool("IsMove", false);
                break;
            case MonsterChaseState chase:
                _animator.SetBool("IsMove", true);
                break;
            case MonsterReturnState goBack:
                _animator.SetBool("IsMove", true);
                break;
        }
    }

    public void DetectTarget(GameObject target)
    {
        _target= target;
    }

    public void MissTarget()
    {
        _target = null;
    }
}
