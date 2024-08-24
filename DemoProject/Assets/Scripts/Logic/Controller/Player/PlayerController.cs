using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _isInvincible = false;
    private bool _isDash = false;

    [SerializeField] private Stat _playerStat;
    [SerializeField] private GameObject _afterImage;

    private float _currentHP;
    private float _currentSpeed;
    private float _jumpForce = 10f;

    private float _moveX;
    private float _moveY;

    private float _afterImageDelay = 0.05f;
    private float _afterImageDelayTime;
    private bool _isAfterImage;
    private float _afterImageTime = 0.7f;
    private float _afterImageTimeLeft = 0.7f;

    private float _dashDistance = 3f;
    private float _dashTime = 0.1f;
    private float _dashTimeLeft;
    private float _lastDashTime = -Mathf.Infinity;
    private float _dashCoolDown = 0.04f;
    private Vector2 _dashDirection= Vector2.zero;

    public Rigidbody2D Rigidbody { get { return _rigidbody; } }
    public Stat PlayerStat { get { return _playerStat; } }

    private void Awake()
    {
        StateMachine = new PlayerStateMachine(this);

        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider= GetComponent<BoxCollider2D>();

        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
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
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ShootBullet();  
        }

        Move();
        Dash();
        MakeAfterImage();

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

    public void Move()
    {
        if (StateMachine.CurrentState == StateMachine.PlayerKnockbackState) 
            return;

        _moveX = Input.GetAxisRaw("Horizontal") * _currentSpeed;
        _moveY = Input.GetAxisRaw("Vertical") * _currentSpeed;

        if (_isDash)
            return;

        Vector2 targetVelocity = new Vector2(_moveX, _moveY);
        _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, targetVelocity, 0.9f); // 0.1f는 보간 속도

        if (_moveX != 0 || _moveY != 0)
        {
            transform.rotation = (_moveX >= 0) ?
                Quaternion.Euler(Vector3.zero) : Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
    }
    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            _rigidbody.velocity = Vector2.zero;
            Vector2 dir = Vector2.up * _jumpForce;
            _rigidbody.AddForce(dir, ForceMode2D.Impulse);
        }
    }
    public void Die()
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
                    StartDash();
                }
                else
                {
                    Debug.Log($"Time Left = {(_lastDashTime + _dashCoolDown) - Time.time}");
                }
            }
        }
       
    }

    public void TakeDamage(GameObject hitSource, float damage)
    {
        if (_isInvincible)
            return;

        _currentHP -= damage;
        if (_currentHP <= 0)
        {
            Die();
        }

        KnockBack(hitSource, Define.KNOCKBACK_FORCE);
    }

    private void StartDash()
    {
        _isDash = true;
        _afterImageTimeLeft = 0f;
        _dashTimeLeft = _dashTime;
        _lastDashTime = Time.time;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX == 0 && moveY == 0)
        {
            _dashDirection = transform.right; // 정지 상태에서는 캐릭터가 보는 방향으로 대쉬
        }
        else
        {
            _dashDirection = new Vector2(moveX, moveY).normalized;
        }
    }

    private void MakeAfterImage()
    {
        if (_afterImageTimeLeft >= _afterImageTime)
        {
            return;
        }

        _afterImageTimeLeft += Time.deltaTime;

        if (_afterImageDelayTime > 0)
            _afterImageDelayTime -= Time.deltaTime;
        else
        {
            GameObject afterimage = Managers.Resource.Instantiate("Character/PlayerAfterImage", Define.AFTER_IMAGE_POOL_COUNT);
            SpriteRenderer afterImageRenderer = afterimage.GetComponentInChildren<SpriteRenderer>();
            afterimage.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
            afterimage.transform.rotation = transform.rotation;
            Sprite currentSprite = GetComponentInChildren<SpriteRenderer>().sprite;
            afterimage.transform.localScale = transform.localScale;
            afterImageRenderer.sprite = currentSprite;
            _afterImageDelayTime = _afterImageDelay;

            afterImageRenderer.DOFade(0.2f, 0.3f);
            StartCoroutine(Managers.Resource.Destroy(afterimage, 0.3f));
        }
    }

    private void ShootBullet()
    {
        var bullet = Managers.Resource.Instantiate("Bullet");
        bullet.GetComponent<Bullet>()?.Init();
    }

    private void KnockBack(GameObject hitSource, float force)
    {
        // 피격 시 Move 함수를 무력화할 필요가 있음
        StateMachine.TransitionTo(StateMachine.PlayerKnockbackState);

        // 플레이어 넉백
        Vector2 direction = transform.position - hitSource.transform.position;

        float minDistance = 0.1f;

        if (direction.magnitude < minDistance)
        {
            direction = transform.right;
        }

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(direction.normalized * force, ForceMode2D.Impulse);

        // alpha 조정
        _isInvincible = true;
        _spriteRenderer.DOFade(0, 0.5f)
                       .SetLoops(6, LoopType.Yoyo)
                       .OnComplete(() => _isInvincible = false);
    }
}
