using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Weapon ParentWeapon;

    private Rigidbody2D _rigidbody;

    private ParticleSystem _trailEffect;
    private ParticleSystem _splashEffect;
    private Collider2D _coreCollider;

    private Transform _homingTarget;
    private float _homingTime;

    private float _limitedLifeTime = 1.3f;
    private float _currentLifeTime = 0f;

    public float HomingTime => _homingTime;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _coreCollider = GetComponent<Collider2D>();
        
    }

    private void Update()
    {
        _currentLifeTime += Time.deltaTime;
        if (_currentLifeTime >= _limitedLifeTime)
        {
            _currentLifeTime = 0f;
            Managers.Resource.Destroy(this.gameObject);
            return;
        }

        if (_homingTarget != null)
            transform.position += transform.right * 0.05f;
    }

    public void Shooting()
    {
        _homingTarget = (ParentWeapon.Owner.TryGetComponent(out Enemy enemy)) ? enemy.EnemyTarget : null;

        if (_homingTarget != null)
        {
            transform.LookAtTarget(_homingTarget);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Entity entity))
        {
            if (entity is Player)
            {
                entity.GetComponent<Player>().TakeDamage(ParentWeapon.Owner.DamageTaken);
            }
            else if (entity is Enemy)
            {
                entity.GetComponent<Enemy>().TakeDamage(ParentWeapon.Owner.DamageTaken);
            }
        }
    }
}
