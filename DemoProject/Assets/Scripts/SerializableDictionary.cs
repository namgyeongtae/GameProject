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
            // 필요할 때만 생성하여 참조를 반환
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
            // Key가 존재하지 않으면 예외를 던지도록 설정
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
                // 이미 있는 키의 값을 변경
                Dictionary[key] = value;
                int index = _keys.IndexOf(key);
                _values[index] = value;
            }
            else
            {
                // 새 키-값 쌍 추가
                Dictionary[key] = value;
                _keys.Add(key);
                _values.Add(value);
            }
        }
    }

    public void OnBeforeSerialize()
    {
        // 직렬화 전, dictionary가 null이 아닌 경우에만 keys와 values 업데이트
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
        // 역직렬화 후에 dictionary를 null로 설정해 필요할 때 동적으로 생성되도록 함
        dictionary = null;
    }
}
