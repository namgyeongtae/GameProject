using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static INode;

public class Enemy : Entity, IDamageable
{
    public GameObject[] _possibleDropItems;

    protected float _detectRange;
    protected float _attackRange;
    protected float _wanderRange = 2;
    protected float _giveUpTime;
    protected float _waitTime;

    protected Vector3 _spawnPoint;
    protected Vector3 _target;
    protected Vector3 _prevTarget;

    protected Vector3 _wanderPoint;
    protected bool _spawned;

    protected Transform _enemyTarget;

    // protected NavMeshAgent _navmeshAgent;

    protected bool _isAttacking;

    public Transform EnemyTarget => _enemyTarget;

    protected override void Awake()
    {
        base.Awake();

        //_navmeshAgent = GetComponent<NavMeshAgent>();

        //if (_navmeshAgent != null)
        //{
        //    _navmeshAgent.updateRotation = false;
        //    _navmeshAgent.updateUpAxis = false;
        //}
    }

    protected override void Start()
    {
        base.Start();

        if (_spritesInGFX.Count > 0)
        {
            _originShader = _spritesInGFX[0].material.shader;
            foreach (var sprite in _spritesInGFX)
                sprite.color = new Color(0, 0, 0, 0);
        }
        else
        {
            _originShader = GFX.GetComponent<SpriteRenderer>().material.shader;
            GFX.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        _coreCollder.enabled = false;
        CanMove = false;
        _target = transform.position;
        _spawnPoint = transform.position;
        _wanderPoint = _spawnPoint + (Vector3)Random.insideUnitCircle * _wanderRange;
        _waitTime = Random.Range(2, 6);

        Spawn();

        _maxHP = _currentHP = _stat.hp;

        if (IsArmed) Armed();
    }

    protected virtual NodeState Wander()
    {
        if (_waitTime > 0)
        {
            _animator.SetBool("IsMove", false);
            _waitTime -= Time.deltaTime;
        }
        else
        {
            if (IsArmed) Armed();

            if (Vector2.Distance(transform.position, _wanderPoint) > 1f)
            {
                _animator.SetBool("IsMove", true);

                _target = _wanderPoint;

                //_navmeshAgent.enabled = true;
                //_navmeshAgent.speed = 1f;
                //_navmeshAgent.isStopped = false;

                //if (!_prevTarget.Equals(_target))
                //{
                //    _navmeshAgent.SetDestination(_target);
                //    _prevTarget = _target;
                //}

                Vector3 dir = _target - transform.position;

                transform.position += dir * 0.7f * Time.deltaTime;

                bool flip = (transform.position.x > _target.x) ? false : true;
                FlipSprite(flip);
            }
            else
            {
                _animator.SetBool("IsMove", false);

                if (_waitTime > 0) 
                    _waitTime -= Time.deltaTime;
                else
                {
                    Vector3 nextPos = _spawnPoint + (Vector3)Random.insideUnitCircle * _wanderRange;
                    _wanderPoint = SearchWanderPoint(nextPos);
                    _waitTime = Random.Range(2, 6);
                }
            }
        }

        return NodeState.Success;
    }

    public void TakeDamage(DamageTaken damageTaken)
    {
        if (!_spawned) return;

        if (!IsArmed && _weapon.WeaponType == Define.WeaponType.Sword) Armed();

        if (damageTaken.KnockBackForce > 0)
            StartCoroutine(KnockBack(damageTaken));

        _currentHP -= damageTaken.DamageAmount;

        foreach (SpriteRenderer renderer in _spritesInGFX)
            renderer.material.shader = _hitEffectShader;

        if (_currentHP <= 0)
        {
            Die();
        }
        else
        {
            CancelInvoke("BackToOriginShader");
            Invoke("BackToOriginShader", 0.4f);
        }
    }

    private void DropItems(int min, int max)
    {
        int random = Random.Range(min, max);

        for (int i = 0; i < random; i++)
        {
            var go = Managers.Resource.Instantiate($"Combat/{_possibleDropItems[Random.Range(0, _possibleDropItems.Length)].name}");
            go.transform.position = transform.position;
            go.transform.rotation = Quaternion.identity;
        }
    }
    
    private void Die()
    {
        int random = Random.Range(Define.XP_MIN, Define.XP_MAX);

        Managers.XP.AddXP(10);

        DropItems(Define.DROP_MIN, Define.DROP_MAX);
        Managers.Resource.Destroy(this.gameObject);
    }

    private void Spawn()
    {
        if (_spritesInGFX.Count > 0) 
            StartCoroutine(CoSpawn());
    }

    private IEnumerator CoSpawn()
    {
        YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();

        foreach (SpriteRenderer sprite in _spritesInGFX)
        {
            sprite.material.shader = _hitEffectShader;
            sprite.color = Color.black;
        }

        float alpha = 0;

        // From invisible to black
        while (alpha < 1)
        {
            alpha += 0.1f;
            foreach (SpriteRenderer sprite in _spritesInGFX)
                sprite.color = new Color(0, 0, 0, alpha);

            yield return waitForFixedUpdate;
        }

        // Form black to white
        alpha = 0;
        while (alpha < 1)
        {
            alpha += 0.2f;
            foreach (SpriteRenderer sprite in _spritesInGFX)
                sprite.color = new Color(alpha, alpha, alpha, 1);

            yield return waitForFixedUpdate;
        }

        // From white to normal
        foreach (SpriteRenderer sh in _spritesInGFX)
            sh.material.shader = _originShader;

        alpha = 0;
        while (alpha < 1)
        {
            alpha += .2f;
            foreach (SpriteRenderer sh in _spritesInGFX)
                sh.color = new Color(alpha, alpha, alpha, 1);

            yield return waitForFixedUpdate;
        }

        yield return new WaitForSeconds(0.2f);

        _coreCollder.enabled = true;
        CanMove = true;
        _spawned = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Player playerController))
        {
            collision.collider.GetComponent<Player>().TakeDamage(_damageTaken);
        }
    }

    private Vector3 SearchWanderPoint(Vector3 targetPosition, float searchRadiusIncrement = 1.0f, float maxSearchRadius = 70f)
    {
        NavMeshHit hit;
        float searchRadius = searchRadiusIncrement;

        while (searchRadius <= maxSearchRadius)
        {
            if (NavMesh.SamplePosition(targetPosition, out hit, searchRadius, NavMesh.AllAreas))
            {
                // 유효한 NavMesh 위치를 찾았을 때 반환
                return hit.position;
            }
            // 검색 반경을 증가시켜 다시 시도
            searchRadius += searchRadiusIncrement;
        }

        // 유효한 위치를 찾지 못했을 경우 원래 위치를 반환하거나 예외 처리
        Debug.LogWarning("유효한 NavMesh 위치를 찾지 못했습니다.");
        return targetPosition;
    }
}
