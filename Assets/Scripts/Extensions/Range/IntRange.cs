using System;
using UnityEngine;
[Serializable]
public struct IntRange {
    [SerializeField]
    private int _min;
    [SerializeField]
    private int _max;
    public int min { get; private set; }
    public int max { get; private set; }
    public int Get() {
        return UnityEngine.Random.Range(_min, _max);
    }
}