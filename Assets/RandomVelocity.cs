using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RandomVelocity : MonoBehaviour {

	[SerializeField]
	private float maxSpeed = 1f;

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().velocity = Random.insideUnitCircle * maxSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
