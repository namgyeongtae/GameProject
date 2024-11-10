using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Security.Cryptography;

public class Player : Entity, IDashable, IDamageable
{
    public PlayerStateMachine StateMachine { get; private set; }

    private BoxCollider2D _boxCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private bool _isDash = false;

    // [SerializeField] private GameObject _afterImage;

    private float _currentSpeed;

    // Move Input (Horizontal, Vertical)
    private float _moveX;
    private float _moveY;

    // Dash After Image
    private float _afterImageDelay = 0.02f;
    private float _afterImageDelayTime;
    private bool _isAfterImage;
    private float _afterImageTime = 0.4f;
    private float _afterImageTimeLeft = 0.4f;

    // Dash
    private float _dashDistance = 3f;
    private float _dashTime = 0.1f;
    private float _dashTimeLeft;
    private float _lastDashTime = -Mathf.Infinity;
    private float _dashCoolDown = 0.04f;
    private Vector2 _dashDirection= Vector2.zero;

    // Effect
    [SerializeField] private ParticleSystem _smokeDashEffect;
    
    public Rigidbody2D Rigidbody { get { return _rigidbody; } }

    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine(this);

        _boxCollider= GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        StateMachine.OnInit();
    }

    public void Init()
    {
        // Init Stat
        var levelData = Managers.Data.XP.XP_Dict[Managers.Character.UserData.Level];

        _currentHP = Managers.Character.UserData.Character.HP;
        _currentSpeed = Managers.Character.UserData.Character.Speed;

        // Init UI
        var playerUI = Managers.UI.GetUI<UIPlayer>();
        playerUI.SetHPFull(levelData.hp);
        playerUI.UpdateLvl(levelData.level);
        playerUI.UpdateXP(Managers.Character.UserData.Exp, levelData.exp_required);
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!CanMove || Knockbacked)
            return;

        Move();
        LookRotation();
        Dash();
        // MakeAfterImage();

        StateMachine.OnUpdate();
    }

    public void SetAnimation(IState state)
    {
        switch (state)
        {
            case PlayerIdleState idle:
                _animator.SetFloat("velocityX", 0);
                _animator.SetFloat("velocityY", 0);
                break;
            case PlayerRunState run:
                _animator.SetFloat("velocityX", new Vector2(_moveX, _moveY).magnitude);
                _animator.SetFloat("velocityY", 0);
                break;
        }
    }

    public override void Move()
    {
        if (!CanMove)
            return;

        if (StateMachine.CurrentState == StateMachine.PlayerKnockbackState) 
            return;

        _moveX = Input.GetAxisRaw("Horizontal") * _currentSpeed;
        _moveY = Input.GetAxisRaw("Vertical") * _currentSpeed;

        if (_isDash)
            return;

        Vector2 targetVelocity = new Vector2(_moveX, _moveY);
        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, targetVelocity, 0.9f); // 0.1f는 보간 속도
    }

    public override void LookRotation()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 look = position - transform.position;

        transform.rotation = (look.x >= 0) ? Quaternion.Euler(Vector3.zero) : Quaternion.Euler(new Vector3(0f, 180f, 0f));
    }

    private void Die()
    {
        // TODO 
        // GameManager로 하여금 게임오버 상태로 진입 시키기
        GameManager.Instance.GameOver();
    }

    public void Dash()
    {
        if (_isDash)
        {
            if (_dashTimeLeft > 0)
            {
                _rigidbody.velocity = _dashDirection * (_dashDistance / _dashTime);
                _dashTimeLeft -= Time.deltaTime;
            }
            else
            {
                _isDash = false;
                _rigidbody.velocity = Vector2.zero;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (Time.time >= (_lastDashTime + _dashCoolDown))
                {
                    StartCoroutine(StartDash());
                }
            }
        }
    }

    private IEnumerator StartDash()
    {
        _isDash = true;
        _dashTimeLeft = _dashTime;
        _lastDashTime = Time.time;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX == 0 && moveY == 0)
            _dashDirection = transform.right; // 정지 상태에서는 캐릭터가 보는 방향으로 대쉬
        else
            _dashDirection = new Vector2(moveX, moveY).normalized;

        _smokeDashEffect.Play();
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(_dashDirection.normalized * 15, ForceMode2D.Impulse);
        yield return new WaitForSecondsRealtime(0.1f);
        _smokeDashEffect.Stop();
    }

    public void TakeDamage(DamageTaken damageTaken)
    {
        if (Invincible) return;

        if (damageTaken.KnockBackForce > 0)
            StartCoroutine(KnockBack(damageTaken));

        _currentHP -= damageTaken.DamageAmount;

        Managers.UI.GetUI<UIPlayer>().TakeDamage(damageTaken.DamageAmount);

        foreach (SpriteRenderer renderer in _spritesInGFX)
            renderer.material.shader = _hitEffectShader;

        if (_currentHP <= 0)
        {
            Invincible = true;
        }
        else
        {
            StartCoroutine(Invincibility());
        }
    }

    /// <summary>
    /// 피격 시 무적상태를 활성화 하고 캐릭터 Sprite에 Whilte Flickering 연출효과
    /// </summary>
    private IEnumerator Invincibility()
    {
        Invincible = true;

        yield return new WaitForSecondsRealtime(.3f);
        int ticks = 10;
        int alpha;
        float time = 0.05f;

        while (ticks > 0)
        {
            if (ticks % 2 == 0)
            {
                alpha = 0;
                if (time > 0.06f) time -= 0.03f;
            }
            else alpha = 1;

            foreach (SpriteRenderer sprite in _spritesInGFX) sprite.color = new Color(1, 1, 1, alpha);
            
            yield return new WaitForSecondsRealtime(time);
            ticks--;
        }

        Invincible = false;

        CancelInvoke("BackToOriginShader");
        Invoke("BackToOriginShader", 0.4f);
    }
}
