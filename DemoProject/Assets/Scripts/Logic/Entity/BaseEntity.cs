using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public Action OnTarget_Event = null;
    public Action OnTargetCancel_Event = null;

    private SpriteRenderer _interactionUI;

    private void Awake()
    {
        _interactionUI = transform.Find("Interaction").GetComponent<SpriteRenderer>();

        OnTarget_Event -= OnTarget;
        OnTarget_Event += OnTarget;
        OnTargetCancel_Event -= OnTargetCancel;
        OnTargetCancel_Event += OnTargetCancel;
    }

    public virtual void OnTarget()
    {
        Debug.Log("OnTarget Invoke!!");
        _interactionUI.transform.DOLocalMoveY(1.0f, 0.3f);
        _interactionUI.DOFade(1.0f, 0.3f);
    }

    public virtual void OnTargetCancel()
    {
        Debug.Log("OnTarget_Cancel Invoke!!");
        _interactionUI.transform.DOLocalMoveY(0.0f, 0.3f);
        _interactionUI.DOFade(0.0f, 0.3f);
    }

    public virtual void Interaction()
    {
        _interactionUI.transform.DOKill();
        _interactionUI.DOKill();

        // player.SetTargetEntity(null);

        this.gameObject.SetActive(false);
    }
}
