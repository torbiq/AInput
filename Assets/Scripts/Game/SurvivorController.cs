using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using System;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class SurvivorController : MonoBehaviour {
    public enum CharacterState {
        Walking,
        Running,
        Idle,
    }
    private CharacterState _state;
    public CharacterState State {
        get { return _state; }
        set {
            _state = value;
            if (OnStateChangedEvent != null) {
                OnStateChangedEvent(_state);
            }
        }
    }
    public event Action<CharacterState> OnStateChangedEvent;
    private void OnStateChangedEventHandler(CharacterState state) {
        switch (state) {
            case CharacterState.Walking:
                _bodyController.SetAnimation("move");
                _legsController.SetAnimation("walk");
                break;
            case CharacterState.Running:
                _bodyController.SetAnimation("move");
                _legsController.SetAnimation("run");
                break;
            case CharacterState.Idle:
                _bodyController.SetAnimation("idle");
                _legsController.SetAnimation("idle");
                break;
            default:
                break;
        }
    }

    private Collider2D _colider2d;
    [SerializeField]
    private Transform _character;
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _rotationSpeed = 5f;
    [SerializeField]
    private float _moveSmooth = 0.5f;
    [SerializeField]
    private float _rotationSmooth = 0.5f;
    [SerializeField]
    private VirtualJoystick _moveJoystick;
    [SerializeField]
    private SpriteAnimationController _bodyController;
    [SerializeField]
    private SpriteAnimationController _legsController;
    
    private Rigidbody2D _ridgidbody2D;
    //private Vector2 _currentRotationInputSpeed;
    private Vector2 _currentMoveSpeed;

    private BaseWeapon _weapon;

    private void Awake() {
        OnStateChangedEvent += OnStateChangedEventHandler;
        State = CharacterState.Idle;
        _ridgidbody2D = GetComponent<Rigidbody2D>();
        _colider2d = GetComponent<Collider2D>();
    }

    // Use this for initialization
    void Start () {
        //_weapon = new AK47(transform, 30, 300);
	}
	
	void Update () {
        var inputDir = _moveJoystick.GetDirection();
        _currentMoveSpeed = Vector2.Lerp(_currentMoveSpeed, inputDir * _moveSpeed, _moveSmooth * Time.deltaTime);
        float msSqrMag = _currentMoveSpeed.sqrMagnitude;
        Debug.Log(inputDir);

        if (inputDir.sqrMagnitude > 0f) {
            _character.LookAt2D(inputDir);
            _ridgidbody2D.MovePosition(transform.position + (Vector3)_currentMoveSpeed * Time.deltaTime);
        }

        if (msSqrMag >= 2f) {
            if (State != CharacterState.Running) {
                State = CharacterState.Running;
            }
        }
        else if (msSqrMag < 2f && msSqrMag > 0.05f) {
            if (State != CharacterState.Walking) {
                State = CharacterState.Walking;
            }
        }
        else if (msSqrMag <= 0.05f) {
            if (State != CharacterState.Idle) {
                State = CharacterState.Idle;
            }
        }
    }
}
