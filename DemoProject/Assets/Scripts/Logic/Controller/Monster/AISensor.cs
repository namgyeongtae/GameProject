using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AISensor : MonoBehaviour
{
    public string TargetTag = "Player";
    public Action<GameObject> OnTriggerEnterEvent;
    public Action OnTriggerExitEvent;

    private void DetectTarget()
    {
        var collider = Physics2D.OverlapCircle(transform.position, 4f, 1 << LayerMask.NameToLayer("Player"));

        if (collider != null)
        {
            OnTriggerEnterEvent?.Invoke(collider.gameObject);
        }
        else
        {
            OnTriggerExitEvent?.Invoke();
        }
    }

    public void OnUpdate()
    {
        DetectTarget();
    }
}
