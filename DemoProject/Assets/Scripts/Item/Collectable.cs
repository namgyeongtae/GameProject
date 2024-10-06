using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UnityEngine.Rendering.DebugUI;

public class Collectable : MonoBehaviour
{
    [SerializeField] private Vector2 _spitMinMax;
    [SerializeField] private float _afkTime = 1;
    [SerializeField] private AudioClip _pickUp;

    [SerializeField] private float _frequency;
    [SerializeField] private float _amplitude;
    [SerializeField] private float _offset;

    private Transform _target;
    private Rigidbody2D _rigidbody;
    private Vector2 _posOffset;
    private Vector2 _tempPos;
    
    [SerializeField] private Transform GFX;
    [SerializeField] private bool _spitDown;
    [SerializeField] private CircleCollider2D _circleCollider;

    public CollectableType _typeValue;

    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        _posOffset = GFX.localPosition;
        _offset = Random.Range(-0.2f, 0.2f);

        Spit(_spitDown);
        Invoke("Activate", _afkTime);
    }

    // Update is called once per frame
    void Update()
    {
        _tempPos = _posOffset;
        _tempPos.y += Mathf.Sin((Time.timeSinceLevelLoad + _offset) * Mathf.PI * _frequency) * _amplitude;
        GFX.localPosition = _tempPos;
    }

    private void FixedUpdate()
    {
        if (_target == null) return;

        if (_afkTime > 0) _afkTime -= Time.fixedDeltaTime;
        else
        {
            // ���� �� �÷��̾�� �ڼ�ó�� �̲����� 
            if (Vector2.Distance(transform.position, _target.position) < 1.5f)
                transform.position = Vector3.Lerp(transform.position, _target.position, 10 * Time.fixedDeltaTime);

            if (Vector2.Distance(transform.position, _target.position) < 0.1f)
                Managers.Resource.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// �������� ������ �� ������ ������ ������ �Լ��Դϴ�.
    /// </summary>
    /// <param name="a">ù ��° ����.</param>
    /// <param name="b">�� ��° ����.</param>
    /// <returns>�� ������ ��.</returns>
    private void Spit(bool down)
    {
        Vector2 spitDirection;

        if (down) spitDirection = new Vector2(Random.Range(-1f, 1f), -1);
        else spitDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        float spitForce = Random.Range(_spitMinMax.x, _spitMinMax.y);
        _rigidbody.AddForce(spitDirection * spitForce, ForceMode2D.Impulse);
    }

    private void Activate()
    {
        _circleCollider.enabled = true;

        // ������ Ÿ�Կ� ���� ������ �ִϸ��̼� ���
        switch (_typeValue)
        {
            case CollectableType.CoinSilver: GetComponent<Animator>().Play("CoinSilverIdle", -1, Random.Range(0f, 1f)); break;
            case CollectableType.CoinGold: GetComponent<Animator>().Play("CoinGoldenIdle", -1, Random.Range(0f, 1f)); break;
            case CollectableType.Diamond: GetComponent<Animator>().Play("DiamondIdle", -1, Random.Range(0f, 1f)); break;
        }
    }
}
