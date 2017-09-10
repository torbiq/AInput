using System;
using UnityEngine;

[Serializable]
public class MeleeBehavior : WeaponBehavior {
    public override void Fire() {

    }
    public MeleeBehavior(Transform spawnPoint, float delay, AudioClip[] attackClips) : base(spawnPoint, delay, attackClips) {
    }
}
