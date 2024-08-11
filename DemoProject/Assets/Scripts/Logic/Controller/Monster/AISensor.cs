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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TargetTag))
        {
            Debug.Log($"OnTriggerEnter : {collision.name}");
            OnTriggerEnterEvent?.Invoke(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TargetTag))
        {
            Debug.Log($"OnTriggerExit : {collision.name}");
            OnTriggerExitEvent?.Invoke();
        }
    }
}
