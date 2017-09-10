using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class WeaponBehavior {
    public Transform spawnPoint;
    public float delay;
    public float nextAttackTime;
    public AudioClip[] attackClips;

    public virtual bool CanFire() {
        return Time.time >= nextAttackTime;
    }
    public abstract void Fire();

    protected WeaponBehavior(Transform spawnPoint, float delay, AudioClip[] attackClips) {
        this.spawnPoint = spawnPoint;
        this.delay = delay;
        this.nextAttackTime = 0f;
        this.attackClips = attackClips;
    }
}
