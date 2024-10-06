using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowWeapon : Weapon
{
    [SerializeField] private Transform _shootPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack()
    {
        StartCoroutine(ShootArrow());
    }

    public IEnumerator ShootArrow()
    {
        yield return new WaitForSeconds(0.4f);

        Projectile arrow = Managers.Resource.Instantiate("Combat/Arrow", 10).GetComponent<Projectile>();
        arrow.ParentWeapon = this;
        Physics2D.IgnoreCollision(arrow.ParentWeapon.Owner.CoreCollider, arrow.GetComponent<Collider2D>(), true);

        arrow.transform.position = _shootPoint.position;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.rotation.eulerAngles.z));
        arrow.Shooting();
    }
}
