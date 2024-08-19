using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pool
{
    public GameObject Original { get; private set; }
    public Transform Root { get; set; }

    private Stack<Poolable> _poolStack = new Stack<Poolable>();

    private int _poolCount = 5;

    public void Init(GameObject original, int count = 5)
    {
        Original = original;

        Root = new GameObject().transform;
        Root.name = $"{original.name}_Root";

        for (int i = 0; i < count; i++)
        {
            Push(Create());
        }
    }

    private Poolable Create()
    {
        GameObject go = Object.Instantiate<GameObject>(Original);
        go.name = Original.name;
        return go.GetOrAddComponent<Poolable>();
    }

    // 사용 중인 Pool 오브젝트를 대기 칸에 넣기
    public void Push(Poolable poolable)
    {
        if (poolable == null)
            return;

        if (_poolStack.Count >= _poolCount)
        {
            Object.Destroy(poolable.gameObject);
            return;
        }

        poolable.transform.parent = Root;
        poolable.gameObject.SetActive(false);
        poolable.IsUsing = false;

        _poolStack.Push(poolable);
    }

    public Poolable Pop(Transform parent)
    {
        Poolable poolable;

        if (_poolStack.Count > 0)
            poolable = _poolStack.Pop();
        else
            poolable = Create();

        poolable.gameObject.SetActive(true);

        Debug.Log(poolable.gameObject.name);

        if (parent == null)
            poolable.transform.parent = GameObject.Find("@PoolObjects").transform;

        poolable.transform.parent = parent;
        poolable.IsUsing = true;

        return poolable;
    }
}
