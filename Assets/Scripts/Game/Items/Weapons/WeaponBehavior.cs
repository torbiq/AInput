using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class WeaponBehavior {
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private float _delay;
    [SerializeField]
    private float _nextAttackTime;
    [SerializeField]
    private AudioClip[] _attackClips;

    public virtual bool CanFire() {
        return Time.time >= _nextAttackTime;
    }
    public abstract void Fire();

    protected WeaponBehavior(Transform spawnPoint, float delay, AudioClip[] attackClips) {
        _spawnPoint = spawnPoint;
        _delay = delay;
        _nextAttackTime = 0f;
        _attackClips = attackClips;
    }
}
