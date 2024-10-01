using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    private float _hp;

    public MonsterStateMachine StateMachine { get; private set; }
    public float HP => _hp;
    public GameObject Target => _target;

    private void Awake()
    {
        StateMachine = new MonsterStateMachine(this);

        _navmeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _aiSensor = GetComponentInChildren<AISensor>();
        _aiSensor.GetComponent<CircleCollider2D>().radius = _monsterStat.detectRange;
        BindAIEvents();

        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StateMachine.OnInit();

        _originPosition = transform.position;
        _hp = _monsterStat.hp;

        _navmeshAgent.updateRotation = false;
        _navmeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        _aiSensor.OnUpdate();

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

        if (_originPosition.x >= transform.position.x)
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        else
            transform.rotation = Quaternion.identity;
    }

    private void Chase()
    {
        _navmeshAgent.isStopped = false;
        _navmeshAgent.speed = _monsterStat.speed;
        _navmeshAgent.SetDestination(_target.transform.position);

        if (_target.transform.position.x >= transform.position.x)
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        else
            transform.rotation = Quaternion.identity;
    }

    public virtual void TakeDamage(GameObject hitSource, float damage)
    {
        damage -= _monsterStat.defense;

        _hp -= damage;

        StartCoroutine(KnockBack(hitSource));
    }

    private IEnumerator KnockBack(GameObject hitSource)
    {
        _navmeshAgent.isStopped = true;

        float knockBackForce = 2.5f;

        switch (StateMachine.CurrentState)
        {
            case MonsterIdleState idle:
                knockBackForce = 2.5f;
                break;
            case MonsterChaseState chase:
            case MonsterReturnState returnState:
                knockBackForce = 6.0f;
                break;
        }

        Vector2 direction = transform.position - hitSource.transform.position;
        _rigidbody.AddForce(direction.normalized * knockBackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(5f);

        _navmeshAgent.isStopped = false;
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
        if (_target != target)
            _target= target;
    }

    public void MissTarget()
    {
        _target = null;
    }

    public bool IsReturnToOrigin()
    {
        if (Vector2.Distance(transform.position, _originPosition) < 0.1f)
            return true;

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            player.TakeDamage(this.gameObject, _monsterStat);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            player.TakeDamage(this.gameObject, _monsterStat);
        }
    }
}
