using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour, IMovable
{
    [SerializeField] protected GameObject GFX;
    [SerializeField] protected Stat _stat;

    [Header("Arm Manager")]
    [SerializeField] protected Transform _handL;
    [SerializeField] protected Transform _handR;
    [SerializeField] protected Transform _back;
    [SerializeField] protected Transform _weaponCenter;
    [SerializeField] protected Transform _offHand;

    public Transform HandL => _handL;
    public Transform HandR => _handR;
    public Transform Back => _back;
    public Transform WeaponCenter => _weaponCenter;
    public Transform OffHand => _offHand;

    [SerializeField] protected Weapon _weapon;
    
    protected float _currentHP;
    protected float _maxHP;

    public bool Knockbacked = false;
    public bool CanMove = true;
    public bool IsArmed = false;
    public bool Invincible = false;
    public bool IsFlipped = false;

    protected List<SpriteRenderer> _spritesInGFX = new();

    protected Rigidbody2D _rigidbody;
    protected Animator _animator;
    protected BoxCollider2D _coreCollder;
    protected Shader _originShader;
    protected Shader _hitEffectShader;

    protected DamageTaken _damageTaken;

    public BoxCollider2D CoreCollider => _coreCollder;
    public float CurrentHP { get { return _currentHP; } set { _currentHP = value; } }
    public Stat Stat => _stat;
    public DamageTaken DamageTaken => _damageTaken;

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _coreCollder = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _spritesInGFX = GFX.GetComponentsInChildren<SpriteRenderer>().ToList();
        _hitEffectShader = Shader.Find("GUI/Text Shader");
        _originShader = _spritesInGFX.FirstOrDefault()?.material.shader;

        _damageTaken = new DamageTaken(_stat.attack, _stat.knockBackForce, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F2))
            Armed();
    }

    private void FixedUpdate()
    {
        if (Knockbacked || !CanMove) return;
    }

    public virtual void Move() { }

    public virtual void LookRotation() { }

    protected virtual IEnumerator KnockBack(DamageTaken damageTaken)
    {
        Knockbacked = true;

        Vector2 direction = transform.position - damageTaken.Source.transform.position;

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(direction.normalized * damageTaken.KnockBackForce * _stat.knockBackMultiplier, ForceMode2D.Impulse);

        // 넉백 효과 유지 시간
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + 0.5f)
        {
            yield return null;
        }

        _rigidbody.velocity = Vector2.zero;

        // 추가 넉백 효과 무시 시간
        startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + 0.2f)
        { 
            yield return null; 
        }

        Knockbacked = false;
    }

    public void BreakKnockback()
    {
        StopCoroutine("KnockBack");
        _rigidbody.velocity = Vector2.zero;
        Knockbacked = false;
    }

    private void BackToOriginShader()
    {
        foreach (SpriteRenderer renderer in _spritesInGFX)
        {
            renderer.material.shader = _originShader;
        }

        Invincible = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Knockbacked) BreakKnockback();
    }

    public void Armed()
    {
        if (_weapon.WeaponType != Define.WeaponType.Sword)
            return;

        IsArmed = !IsArmed;

        _weapon.Holster();
    }

    public void FlipSprite(bool flip)
    {
        if (flip == IsFlipped) return;

        if (flip)
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        else
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        IsFlipped = flip;
    }
}
 