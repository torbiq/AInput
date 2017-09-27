using System;
using UnityEngine;
[Serializable]
public class KeyValuePaired<TKey, TValue> {
    [SerializeField]
    private TKey _key;
    [SerializeField]
    private TValue _value;
    public TKey key { get { return _key; } set { _key = value; } }
    public TValue value { get { return _value; } set { _value = value; } }
} 
