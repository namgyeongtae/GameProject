using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public abstract class UIBase : MonoBehaviour
{
    private Dictionary<Type, UnityEngine.Object[]> _objects = new();
    private bool _isInitialize = false;

    private void Start()
    {
        Init();
    }

    public abstract void Init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        var objs = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objs);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objs[i] = Utils.FindChild(gameObject, names[i], true);
            else
                objs[i] = Utils.FindChild<T>(gameObject, names[i], true);
            if (objs[i] == null)
                Debug.LogError($"[UI Base: {name}] Failed to bind: {names[i]}");

        }
    }

    protected void BindObect(Type type) => Bind<GameObject>(type);
    protected void BindImage(Type type) => Bind<Image>(type);
    protected void BindText(Type type) => Bind<Text>(type);
    protected void BindButton(Type type) => Bind<UIButton>(type);

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected Text GetText(int idx) { return Get<Text>(idx); }
    protected UIButton GetButton(int idx) { return Get<UIButton>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    public static void BindEvent(GameObject obj, Action<PointerEventData> action = null, UIEvent type = UIEvent.Click)
    {
        UIEventHandler uiEventHandler = Utils.GetOrAddComponent<UIEventHandler>(obj);

        switch (type)
        {
            case UIEvent.Click:
                uiEventHandler.OnClickHandler -= action;
                uiEventHandler.OnClickHandler += action;
                break;
            case UIEvent.PointerUp:
                uiEventHandler.OnClickUpHandler -= action;
                uiEventHandler.OnClickUpHandler += action;
                break;
            case UIEvent.PointerDown:
                uiEventHandler.OnClickDownHandler -= action;
                uiEventHandler.OnClickDownHandler += action;
                break;
            case UIEvent.BeginDrag:
                uiEventHandler.OnBeginDragHandler -= action;
                uiEventHandler.OnBeginDragHandler += action;
                break;
            case UIEvent.Drag:
                uiEventHandler.OnDragHandler -= action;
                uiEventHandler.OnDragHandler += action;
                break;
            case UIEvent.EndDrag:
                uiEventHandler.OnEndDragHandler -= action;
                uiEventHandler.OnEndDragHandler += action;
                break;
        }
    }
}
