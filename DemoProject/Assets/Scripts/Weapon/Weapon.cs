using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Weapon : MonoBehaviour
{
    protected Vector2 _backOffset = new Vector2(0f, 0.3f);
    protected Vector2 _holdOffset = new Vector2(-0.5f, 0.4f);
    protected float _holdRotation = 180f;
    
    protected bool _isHolstered = false;

    protected BoxCollider2D _coreCollider;

    protected bool _isAttacking;

    [SerializeField] protected WeaponType _weaponType;

    [SerializeField] protected float _attackSpeed;

    [SerializeField] protected SpriteRenderer GFX;
    [SerializeField] protected Transform _pivotPoint;
    
    public Entity Owner;
    public WeaponType WeaponType => _weaponType;

    private void Awake()
    {
        Owner = GetComponentInParent<Entity>();

        _coreCollider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Holster()
    {
        _isHolstered = !Owner.IsArmed;

        if (_coreCollider != null)
            _coreCollider.enabled = !_isHolstered;

        // 장착 해제 상태
        if (_isHolstered)
        {
            transform.parent = Owner.Back;
            transform.localPosition = _backOffset;
            transform.rotation = Quaternion.Euler(Vector3.forward * _holdRotation);
        }
        // 장착 상태 
        else
        {
            transform.parent = Owner.WeaponCenter;
            transform.localPosition = _holdOffset;

            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 60));
        }
    }

    public virtual void Attack()
    {

    }
}
