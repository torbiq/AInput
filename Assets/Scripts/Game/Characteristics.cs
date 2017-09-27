using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[Serializable]
//public class Chartics {
//    [SerializeField]
//    private CharticFloat _hp;
//    [SerializeField]
//    private CharticFloat _stamina;
//    [SerializeField]
//    private CharticFloat _satiety;
//    [SerializeField]
//    private CharticFloat _hydratation;

//    private void Update(float deltaTime) {
//        _hp.Update
//    }
//}

public enum CharticType {
    Health,
    Stamina,
    Hunger,
    Dehydratation,
}

[Serializable]
public class CharticFloat {
    [SerializeField]
    private float _max;
    [SerializeField]
    private float _current;
    [SerializeField]
    private float _changeAmount;

    public UnityEvent OnMin;
    public UnityEvent OnMax;

    public float max { get { return _max; } set { _max = value; } }
    public float current {
        get {
            return _current;
        }
        set {
            float val = Mathf.Max(Mathf.Min(value, _max), 0f);
            if (_current != val) {
                if (val == 0f) {
                    OnMin.Invoke();
                }
                if (val == _max) {
                    OnMax.Invoke();
                }
            }
            _current = val;
        }
    }
    public float changeAmount { get { return _changeAmount; } set { _changeAmount = value; } }

    public void Update(float deltaTime) {
        _current += _changeAmount * deltaTime;
    }
    public CharticFloat(float max, float current, float changeAmount) {
        _max = max;
        _current = current;
        _changeAmount = changeAmount;
    }
    public CharticFloat(CharticFloat chngFloat) {
        _max = chngFloat._max;
        _current = chngFloat._current;
        _changeAmount = chngFloat._changeAmount;
    }

    #region Operator overloading (+, -, *, /)
    public static CharticFloat operator +(CharticFloat left, CharticFloat right) {
        var leftInstance = new CharticFloat(left);
        leftInstance._max += left._max;
        leftInstance._current += right._current;
        leftInstance._changeAmount += right._changeAmount;
        return leftInstance;
    }
    public static CharticFloat operator -(CharticFloat left, CharticFloat right) {
        var leftInstance = new CharticFloat(left);
        leftInstance._max -= left._max;
        leftInstance._current -= right._current;
        leftInstance._changeAmount -= right._changeAmount;
        return leftInstance;
    }
    public static CharticFloat operator *(CharticFloat left, CharticFloat right) {
        var leftInstance = new CharticFloat(left);
        leftInstance._max *= left._max;
        leftInstance._current *= right._current;
        leftInstance._changeAmount *= right._changeAmount;
        return leftInstance;
    }
    public static CharticFloat operator /(CharticFloat left, CharticFloat right) {
        var leftInstance = new CharticFloat(left);
        leftInstance._max /= left._max;
        leftInstance._current /= right._current;
        leftInstance._changeAmount /= right._changeAmount;
        return leftInstance;
    }
    #endregion
}