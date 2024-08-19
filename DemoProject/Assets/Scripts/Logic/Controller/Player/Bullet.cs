using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerController _shooter;

    private Vector3 _direction;
    private float _lifeTime = 5f;
    private float _currentTime = 0f;

    private Vector3 _startPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= _lifeTime)
        {
            Die();
            return;
        }

        Move();
    }

    public void Init()
    {
        _shooter = GameObject.Find("Player").GetComponent<PlayerController>();
        var player = _shooter.transform.Find("ShootPos");

        if (player == null)
            return;

        transform.position = player.transform.position;

        _direction = player.transform.right;

        if (_direction.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;

    }

    private void Move()
    {
        transform.position += _direction.normalized * 0.05f;
    }

    private void Die()
    {
        _currentTime = 0f;
        Managers.Resource.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var monster = collision.GetComponent<Monster>();
        if (monster != null)
            monster.TakeDamage(this.gameObject, _shooter.PlayerStat.attack);

        if (!collision.CompareTag("IgnoreTag")
            && !collision.CompareTag("Player"))
        {
            Debug.Log(collision.name);
            Die();
        }
    }
}
