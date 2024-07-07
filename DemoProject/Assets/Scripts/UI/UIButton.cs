using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

[AddComponentMenu("UI/UIButton", 30)]
public class UIButton : Button
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (isActiveAndEnabled == false)
            return;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isActiveAndEnabled == false)
            return;

        PressEvent();
    }

    public void BindEvent(UnityAction action)
    {
        onClick.RemoveAllListeners();
        onClick.AddListener(action);
    }

    private void PressEvent()
    {
        if (!IsActive() || !IsInteractable()) return;

        onClick?.Invoke();
    }
}
