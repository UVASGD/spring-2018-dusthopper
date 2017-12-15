using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RandomVelocity : MonoBehaviour {

	[SerializeField] private Vector2 speedRange;

	// Use this for initialization
	void Awake () {
		GetComponent<Rigidbody2D> ().velocity = Random.insideUnitCircle * Random.Range(speedRange.x, speedRange.y);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
