using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : Entity
{
    public PlayerStateMachine StateMachine { get; private set; }

    private BoxCollider2D _boxCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private bool _isDash = false;

    [SerializeField] private Stat _playerStat;
    [SerializeField] private GameObject _afterImage;

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
    public Stat PlayerStat { get { return _playerStat; } }

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

        _currentHP = _playerStat.hp;
        _currentSpeed = _playerStat.speed;

        StateMachine.OnInit();
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
        LookRotaiton();
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
            //case PlayerJumpState jump:
            //    _animator.SetFloat("velocityY", _rigidbody.velocity.y);
            //    break;
            //case PlayerFallState fall:
            //    _animator.SetFloat("velocityY", _rigidbody.velocity.y);
            //    break;
        }
    }
    
    public bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 2.0f, 1 << LayerMask.NameToLayer("Ground"));
    }

    private void Move()
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

    private void LookRotaiton()
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

    private void Dash()
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

    public override void TakeDamage(GameObject hitSource, Stat hitterStat)
    {
        if (Invincible) return;

        base.TakeDamage(hitSource, hitterStat);

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
    /// <returns>두 정수의 합</returns>
    /// <exception cref="ArgumentOutOfRangeException">a 또는 b가 허용 범위를 초과할 때 발생합니다.</exception>
    private IEnumerator Invincibility()
    {
        Invincible = true;

        yield return new WaitForSecondsRealtime(.3f);
        int ticks = 10;
        int alpha;
        float time = .2f;

        while (ticks > 0)
        {
            if (ticks % 2 == 0)
            {
                alpha = 0;
                if (time > .06f) time -= .03f;
            }
            else alpha = 1;

            foreach (SpriteRenderer sprite in _spritesInGFX) sprite.color = new Color(1, 1, 1, alpha);
            
            yield return new WaitForSecondsRealtime(time);
            ticks--;
        }

        Invincible = false;
    }
}
