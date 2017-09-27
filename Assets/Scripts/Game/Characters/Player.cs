using Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class Player : AHuman {
    [SerializeField]
    private VirtualJoystick _moveJoystick;
    [SerializeField]
    private CharticFloat _stamina = new CharticFloat(100f, 100f, 2f);
    [SerializeField]
    private CharticFloat _hydratation = new CharticFloat(100f, 100f, -0.01f);
    [SerializeField]
    private CharticFloat _satiety = new CharticFloat(100f, 100f, -0.01f);
    protected override void Awake () {
        base.Awake();
	}
	protected override void Update () {
        base.Update();
        _stamina.Update(Time.deltaTime);
        _hydratation.Update(Time.deltaTime);
        _satiety.Update(Time.deltaTime);

        var inputDir = _moveJoystick.GetDirection() * speed.current;
        float msSqrMag = inputDir.sqrMagnitude;

        if (inputDir.sqrMagnitude > 0f) {
            characterTransform.LookAt2D(inputDir);
            rigidbody.MovePosition(transform.position + (Vector3)inputDir * Time.deltaTime);
        }
        if (msSqrMag >= 10f) {
            if (state != CharacterState.Running) {
                state = CharacterState.Running;
            }
        }
        else if (msSqrMag < 10f && msSqrMag > 0f) {
            if (state != CharacterState.Walking) {
                state = CharacterState.Walking;
            }
        }
        else if (msSqrMag <= 0f) {
            if (state != CharacterState.Idle) {
                state = CharacterState.Idle;
            }
        }
    }
}
