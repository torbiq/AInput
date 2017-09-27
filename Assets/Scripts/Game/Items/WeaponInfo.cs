using System;
using UnityEngine;

[Serializable]
public class WeaponInfo : Info {
    [SerializeField]
    private float _range;
    [SerializeField]
    private FloatRange _accuracy;
    [SerializeField]
    private FloatRange _damage;
    [SerializeField]
    private float _attackDelay;
    [SerializeField]
    private AudioClip[] _attackClips;

    public float range { get { return _range; } set { _range = value; } }
    public FloatRange accuracy { get { return _accuracy; } set { _accuracy = value; } }
    public FloatRange damage { get { return _damage; } set { _damage = value; } }
    public float attackDelay { get { return _attackDelay; } set { _attackDelay = value; } }
    public AudioClip[] attackClips { get { return _attackClips; } set { _attackClips = value; } }
}