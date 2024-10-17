using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : Weapon
{
    private ParticleSystem _slashEffect;

    private float _checkAngle, _pivotAngle;

    private bool _isAlternateTop;
    private float _recoverAngle;
    private float _smoothness;

    private float _targetAngle;
    private Vector3 _desiredRotation;

    public float TargetAngle => _targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        _slashEffect = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack()
    {
        Slash();
    }

    private void Slash()
    {
        _slashEffect.Play();
        _isAlternateTop = !_isAlternateTop;

        _recoverAngle = 135;
        Invoke("TrailOff", 0.2f);

        _isAttacking = true;
    }

    private void TrailOff()
    {
        _slashEffect.Stop();
        _isAttacking = false;
    }

    private int Alternate() // Sword alternate between top and bottom swing
    {
        if (_isAlternateTop) return 1;
        return -1;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_isAttacking) return;

        if (collision.TryGetComponent(out Entity entity))
        {
            if (Owner != entity)
            {
                if (entity is Player) entity.GetComponent<Player>().TakeDamage(this.gameObject, Owner.Stat);
                else if (entity is Enemy) entity.GetComponent<Enemy>().TakeDamage(this.gameObject, Owner.Stat);
            }
        }
    }
}
