using System;
using UnityEngine;

[Serializable]
public abstract class BaseWeapon : Item<WeaponInfo> {
    [SerializeField]
    private WeaponBehavior _attackBehavior;
    public WeaponBehavior attackBehavior { get { return _attackBehavior; } protected set { _attackBehavior = value; } }
    public virtual void Fire() {
        if (attackBehavior != null) {
            attackBehavior.Fire();
        }
    }
}
