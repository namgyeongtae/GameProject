using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static INode;

public class Enemy : Entity
{
    public GameObject[] _possibleDropItems;

    protected float _detectRange;
    protected float _attackRange;
    protected float _wanderRange = 2;
    protected float _giveUpTime;
    protected float _waitTime;

    protected Vector3 _spawnPoint;
    protected Vector3 _target;

    protected Vector3 _wanderPoint;
    protected bool _spawned;

    protected bool _isAttacking;

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

            if (Vector2.Distance(transform.position, _wanderPoint) > 0.1)
            {
                _animator.SetBool("IsMove", true);

                _target = _wanderPoint;

                _navmeshAgent.speed = 1f;
                _navmeshAgent.isStopped = false;
                _navmeshAgent.SetDestination(_target);

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
                    _wanderPoint = _spawnPoint + (Vector3)Random.insideUnitCircle * _wanderRange;
                    _waitTime = Random.Range(2, 6);
                }
            }
        }

        return NodeState.Success;
    }

    public override void TakeDamage(GameObject hitSource, Stat hitterStat)
    {
        if (!_spawned) return;

        if (!IsArmed) Armed();

        base.TakeDamage(hitSource, hitterStat);

        if (_currentHP <= 0)
        {
            DropItems(Define.DROP_MIN, Define.DROP_MAX);
            Managers.Resource.Destroy(this.gameObject);
        }
    }

    public void DropItems(int min, int max)
    {
        int random = Random.Range(min, max);

        for (int i = 0; i < random; i++)
        {
            var go = Managers.Resource.Instantiate($"Combat/{_possibleDropItems[Random.Range(0, _possibleDropItems.Length)].name}");
            go.transform.position = transform.position;
            go.transform.rotation = Quaternion.identity;
        }
    }

    public void Spawn()
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
        if (collision.collider.TryGetComponent(out PlayerController playerController))
        {
            collision.collider.GetComponent<PlayerController>().TakeDamage(this.gameObject, _stat);
        }
    }
}
