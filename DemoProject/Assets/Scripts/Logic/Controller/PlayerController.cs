using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }

    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private float _speed = 5.0f;
    private float _jumpForce = 10f;

    public Rigidbody2D Rigidbody { get { return _rigidbody; } }

    private void Awake()
    {
        StateMachine = new PlayerStateMachine(this);

        _rigidbody= GetComponent<Rigidbody2D>();
        _boxCollider= GetComponent<BoxCollider2D>();

        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StateMachine.OnInit();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();

        Debug.DrawRay(transform.position, Vector2.down * 1.0f, Color.red);

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
                _animator.SetFloat("velocityX", Mathf.Abs(_rigidbody.velocity.x));
                _animator.SetFloat("velocityY", 0);
                break;
            case PlayerJumpState jump:
                _animator.SetFloat("velocityY", _rigidbody.velocity.y);
                break;
            case PlayerFallState fall:
                _animator.SetFloat("velocityY", _rigidbody.velocity.y);
                break;
        }
    }
    
    public bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 2.0f, 1 << LayerMask.NameToLayer("Ground"));
    }

    public void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * _speed;
        _rigidbody.velocity = new Vector2(moveX, _rigidbody.velocity.y);

        if (moveX == 0)
            return;

        _spriteRenderer.flipX = (moveX < 0) ? true : false;
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
}
