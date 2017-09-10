using System;

[Serializable]
public abstract class BaseWeapon : BaseItem {
    public WeaponBehavior attackBehavior { get; protected set; }
    public virtual void Fire() {
        if (attackBehavior != null) {
            attackBehavior.Fire();
        }
    }
}
