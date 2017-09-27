using System;
using System.Collections.Generic;
using UnityEngine;

public enum NPCType {
    Neutral,
    Zombie,
    Killer,
}

[Serializable]
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public abstract class ACharacter<TState> : MonoBehaviour {
    [SerializeField]
    private NPCType _npcType;
    [SerializeField]
    private CharticFloat _health = new CharticFloat(0f, 100f, 0.1f);
    [SerializeField]
    private CharticFloat _speed = new CharticFloat(7f, 7f, 0f);
    [SerializeField]
    private Transform _characterTransform;
    [SerializeField]
    private Collider2D _collider;
    [SerializeField]
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private BaseWeapon _weapon;

    private Vector2 _direction;
    private TState _state;
    public event Action<TState> OnStateChangedEvent;

    public CharticFloat health { get { return _health; } protected set { _health = value; } }
    public CharticFloat speed { get { return _speed; } protected set { _speed = value; } }
    public Vector2 direction { get { return _direction; } protected set { _direction = value; } }
    public TState state { get { return _state; }
        set {
            _state = value;
            if (OnStateChangedEvent != null) {
                OnStateChangedEvent(_state);
            }
        }
    }
    public Transform characterTransform { get { return _characterTransform; } protected set { _characterTransform = value; } }
    public Collider2D collider { get { return _collider; } protected set { _collider = value; } }
    public Rigidbody2D rigidbody { get { return _rigidbody; } protected set { _rigidbody = value; } }
    public BaseWeapon weapon { get { return _weapon; } protected set { _weapon = value; } }
    public NPCType npcType { get { return _npcType; } protected set { _npcType = value; } }
    protected virtual void Awake() {
        if (_collider == null) {
            _collider = GetComponent<Collider2D>();
        }
        if (_rigidbody == null) {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    }
    protected virtual void Start() {

    }
    protected virtual void Update() {
        _health.Update(Time.deltaTime);
        _speed.Update(Time.deltaTime);
    }
    protected virtual void OnDestroy() {

    }
}
