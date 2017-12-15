using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInRadius : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.sqrMagnitude > GameState.fieldRadius * GameState.fieldRadius) {
//			print ("Boom boom");
			Vector2 vel = GetComponent<Rigidbody2D> ().velocity;
			GetComponent<Rigidbody2D>().velocity = Vector2.Reflect (vel, -transform.position.normalized);
		}
	}
}
