using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Utils.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UIBase.BindEvent(go, action, type);
    }

    public static void LookAtTarget(this Transform tr, Transform target)
    {
        float angle = Utils.AngleBetweenTwoPoints(tr.position, target.position);

        tr.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
