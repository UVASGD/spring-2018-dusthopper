using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInRadius : MonoBehaviour {
	//This script bounces an asteroid off the edge of the world (which is a big circle) keeping it from drifting away
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.sqrMagnitude > GameState.fieldRadius * GameState.fieldRadius) {
//			print ("Boom boom");
			Vector2 vel = GetComponent<Rigidbody2D> ().velocity;
			GetComponent<Rigidbody2D>().velocity = Vector2.Reflect (vel, -transform.position.normalized);
		}
	}
}
