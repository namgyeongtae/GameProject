using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Gun _gun;

    private Vector3 _direction;
    private float _lifeTime = 5f;
    private float _currentTime = 0f;

    private DamageTaken _damageTaken;

    // Start is called before the first frame update
    void Start()
    {
        _damageTaken = new DamageTaken(Managers.Character.UserData.Character.Attack,
                                       Managers.Character.UserData.Character.KnockbackForce,
                                       _gun.Player.gameObject);
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

    public void Init(Gun gun)
    {
        _gun = gun;

        var shootPos = _gun.transform.Find("ShootPos");

        if (shootPos == null)
        {
            Debug.LogError("Shoot Pos is NULL!!");
            return;
        }

        _direction = shootPos.transform.right;

        transform.position = shootPos.position;
        transform.right = _direction;


        if (_direction.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;

    }

    private void Move()
    {
        transform.position += _direction.normalized * 0.05f;
        // Debug.Log($"{transform.position} -> {GetComponent<BoxCollider2D>().bounds.center}");
    }

    private void Die()
    {
        _currentTime = 0f;
        Managers.Resource.Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var monster = collision.GetComponent<Enemy>();
        if (monster != null)
        {
            Debug.Log("Monster TakeDamage");
            monster.TakeDamage(_damageTaken);
        }

        var boss = collision.GetComponent<Boss>();
        if (boss != null)
            boss.TakeDamage(_damageTaken);

        if (!collision.CompareTag("IgnoreTag")
            && !collision.CompareTag("Player"))
        {
            Debug.Log(collision.name);
            Die();
        }
    }
}
