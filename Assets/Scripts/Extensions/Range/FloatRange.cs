using System;
using UnityEngine;
[Serializable]
public struct FloatRange : IRange<float> {
    [SerializeField]
    private float _min;
    [SerializeField]
    private float _max;
    public float min { get; private set; }
    public float max { get; private set; }
    public float Get() {
        return UnityEngine.Random.Range(_min, _max);
    }
}
