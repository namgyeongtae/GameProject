using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> _keys = new List<TKey>();
    [SerializeField] private List<TValue> _values = new List<TValue>();

    public List<TKey> Keys => _keys;
    public List<TValue> Values => _values;

    private Dictionary<TKey, TValue> dictionary;

    public Dictionary<TKey, TValue> Dictionary
    {
        get
        {
            // �ʿ��� ���� �����Ͽ� ������ ��ȯ
            if (dictionary == null)
            {
                dictionary = new Dictionary<TKey, TValue>();
                for (int i = 0; i < _keys.Count; i++)
                {
                    dictionary[_keys[i]] = _values[i];
                }
            }
            return dictionary;
        }
    }

    public TValue this[TKey key]
    {
        get
        {
            // Key�� �������� ������ ���ܸ� �������� ����
            if (!Dictionary.ContainsKey(key))
            {
                throw new KeyNotFoundException($"The key '{key}' was not found in the dictionary.");
            }
            return Dictionary[key];
        }
        set
        {
            if (Dictionary.ContainsKey(key))
            {
                // �̹� �ִ� Ű�� ���� ����
                Dictionary[key] = value;
                int index = _keys.IndexOf(key);
                _values[index] = value;
            }
            else
            {
                // �� Ű-�� �� �߰�
                Dictionary[key] = value;
                _keys.Add(key);
                _values.Add(value);
            }
        }
    }

    public void OnBeforeSerialize()
    {
        // ����ȭ ��, dictionary�� null�� �ƴ� ��쿡�� keys�� values ������Ʈ
        if (dictionary != null)
        {
            _keys.Clear();
            _values.Clear();
            foreach (var kvp in dictionary)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
        }
    }

    public void OnAfterDeserialize()
    {
        // ������ȭ �Ŀ� dictionary�� null�� ������ �ʿ��� �� �������� �����ǵ��� ��
        dictionary = null;
    }
}
