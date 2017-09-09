using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class SurvivorController : MonoBehaviour {
    [SerializeField]
    private Rigidbody2D _ridgidbody2D;
    [SerializeField]
    private Collider2D _colider2d;
    [SerializeField]
    private float _moveSpeed = 5f;

    private void Awake() {
        _ridgidbody2D = GetComponent<Rigidbody2D>();
        _colider2d = GetComponent<Collider2D>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A)) {
            _ridgidbody2D.MovePosition(transform.position + _moveSpeed * Time.deltaTime * Vector3.left);
        }
        if (Input.GetKey(KeyCode.D)) {
            _ridgidbody2D.MovePosition(transform.position + _moveSpeed * Time.deltaTime * Vector3.right);
        }
        if (Input.GetKey(KeyCode.W)) {
            _ridgidbody2D.MovePosition(transform.position + _moveSpeed * Time.deltaTime * Vector3.up);
        }
        if (Input.GetKey(KeyCode.S)) {
            _ridgidbody2D.MovePosition(transform.position + _moveSpeed * Time.deltaTime * Vector3.down);
        }
    }
}
