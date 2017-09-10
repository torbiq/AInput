using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : BaseWeapon {
    public AK47(Transform spawnPoint, int magLeft, int ammo) {
        this.name = "AK-47";
        this.description = "The AK-47, or AK as it is officially known is a selective-fire (semi-automatic and fully automatic), gas-operated 7.62×39 mm assault rifle, developed in the Soviet Union by Mikhail Kalashnikov. It is the originating firearm of the 'AK' family.";
        this.weight = 3.9f;
        this.width = 3;
        this.height = 2;
        this.groundPrefabPath = "";
        this.packPrefabPath = "";
        this.attackBehavior = new FirearmsBehavior(spawnPoint: spawnPoint,
            delay: 0.1f,
            nextAttackTime: 0f,
            attackClips: new AudioClip[0],
            realoadDuration: 4f,
            reloadClips: new AudioClip[0],
            magCapacity: 30,
            magLeft: magLeft,
            ammo: ammo,
            maxDistance: 15f);
    }
    public override void Fire() {
        base.Fire();
        Debug.Log("Firing from ak-47");
        Debug.Log("Mag left bullets: " + ((FirearmsBehavior)this.attackBehavior).magLeft);
    }
}
