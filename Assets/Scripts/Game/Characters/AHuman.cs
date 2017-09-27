using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState {
    Walking,
    Running,
    Idle,
    Reload,
    Shoot,
}
public enum WeaponType {
    Flashlight,
    Handgun,
    Knife,
    Rifle,
    Shotgun
}

[Serializable]
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public abstract class AHuman : ACharacter<CharacterState> {
    [SerializeField]
    private SpriteAnimationController _bodyFlashlightController;
    [SerializeField]
    private SpriteAnimationController _bodyHandgunController;
    [SerializeField]
    private SpriteAnimationController _bodyKnifeController;
    [SerializeField]
    private SpriteAnimationController _bodyRifleController;
    [SerializeField]
    private SpriteAnimationController _bodyShotgunController;
    [SerializeField]
    private WeaponType _weaponType;
    [SerializeField]
    private SpriteAnimationController _legsController;

    public event Action<WeaponType> OnWeaponChanged;
    private SpriteAnimationController _bodyController;

    public WeaponType weaponType {
        get {
            return _weaponType;
        }
        set {
            _weaponType = value;
            if (OnWeaponChanged != null) {
                OnWeaponChanged(_weaponType);
            }
        }
    }
    public SpriteAnimationController legsController { get { return _legsController; } set { _legsController = value; } }
    public SpriteAnimationController bodyController { get { return _bodyController; } set { _bodyController = value; } }

    private void OnWeaponTypeChanged(WeaponType weaponType) {
        switch (weaponType) {
            case WeaponType.Flashlight:
                _bodyController = _bodyFlashlightController;
                break;
            case WeaponType.Handgun:
                _bodyController = _bodyHandgunController;
                break;
            case WeaponType.Knife:
                _bodyController = _bodyKnifeController;
                break;
            case WeaponType.Rifle:
                _bodyController = _bodyRifleController;
                break;
            case WeaponType.Shotgun:
                _bodyController = _bodyShotgunController;
                break;
            default:
                _bodyController = _bodyKnifeController;
                break;
        }
    }

    private void OnStateChangedEventHandler(CharacterState state) {
        switch (state) {
            case CharacterState.Walking:
                bodyController.SetAnimation("move");
                legsController.SetAnimation("walk");
                break;
            case CharacterState.Running:
                bodyController.SetAnimation("move");
                legsController.SetAnimation("run");
                break;
            case CharacterState.Idle:
                bodyController.SetAnimation("idle");
                legsController.SetAnimation("idle");
                break;
            default:
                break;
        }
    }

    protected override void Awake() {
        base.Awake();
        OnStateChangedEvent += OnStateChangedEventHandler;
        OnWeaponChanged += OnWeaponTypeChanged;
        weaponType = WeaponType.Flashlight;
        state = CharacterState.Idle;
    }
}
