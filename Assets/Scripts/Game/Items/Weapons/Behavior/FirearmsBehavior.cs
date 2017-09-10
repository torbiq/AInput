using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FirearmsBehavior : WeaponBehavior {
    public float realoadDuration;
    public AudioClip[] reloadClips;
    public int magCapacity;
    public int magLeft;
    public int ammo;
    public float maxDistance;
    public float nextReloadTime;

    public new bool CanFire() {
        return magLeft > 0 && base.CanFire();
    }
    public override void Fire() {
        if (magLeft > 0) {
            if (base.CanFire()) {
                magLeft--;
            }
        }
        else {
            Reload();
        }
    }
    public void Reload() {
        if (ammo > 0) {
            if (magLeft < magCapacity) {
                ammo += magLeft;
                magLeft = 0;
                if (ammo >= magCapacity) {
                    magLeft = magCapacity;
                    ammo -= magCapacity;
                }
                else {
                    magLeft = ammo;
                    ammo = 0;
                }
            }
        }
    }
    public FirearmsBehavior(Transform spawnPoint,
        float delay,
        float nextAttackTime,
        AudioClip[] attackClips,
        float realoadDuration,
        AudioClip[] reloadClips,
        int magCapacity,
        int magLeft,
        int ammo,
        float maxDistance) : base(spawnPoint, delay, attackClips) {
        this.realoadDuration = realoadDuration;
        this.reloadClips = reloadClips;
        this.magCapacity = magCapacity;
        this.magLeft = magLeft;
        this.ammo = ammo;
        this.maxDistance = maxDistance;
        this.nextReloadTime = 0f;
    }
}
