using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinsGameManager : MonoBehaviour {
    [Serializable]
    public class CoinPoint {
        [SerializeField]
        private Transform _coinTransform;
        [SerializeField]
        private float _minForce;
        [SerializeField]
        private float _maxForce;
    }

    [SerializeField]
    private List<CoinPoint> _coinPoints;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
