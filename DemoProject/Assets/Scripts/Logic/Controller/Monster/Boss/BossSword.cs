using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSword : MonoBehaviour
{
    [SerializeField] Stat _stat;

    public void AttackDash(GameObject target)
    {
        Vector2 targetDir = target.transform.position - this.transform.position;

        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        angle += -90f;
        
        var rot = Quaternion.Euler(new Vector3(0f, 0f, angle));

        transform.DORotate(new Vector3(0f, 0f, angle), 0.5f)
                 .OnComplete(() =>
                 {
                     transform.DOMove(target.transform.position, 0.7f)
                              .SetEase(Ease.InSine)
                              .OnComplete(() =>
                              {
                                  Managers.Resource.Destroy(this.gameObject);
                              });
                 });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().TakeDamage(this.gameObject, _stat);
        }
    }
}
