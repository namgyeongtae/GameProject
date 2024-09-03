using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    PlayerController _player;

    public PlayerController Player => _player;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ShootBullet();
        }

        LookRotation();
    }

    private void LookRotation()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePos - transform.position;

        direction.z = 0f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (direction.x < 0f)
            angle = 180f - angle;

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        transform.localRotation = Quaternion.Euler(new Vector3(0f,transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
    }

    private void ShootBullet()
    {
        Bullet bullet = Managers.Resource.Instantiate("Bullet").GetComponent<Bullet>();
        bullet.Init(this);
    }
}
