using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finished
public class StayInRadius : MonoBehaviour {
	//This script bounces an asteroid off the edge of the world (which is a big circle) keeping it from drifting away
	//Also used by hub and gravity fragment asteroids to keep them in roughly the same world position

	public float radius = 0f;
    public Vector3 center = Vector3.zero;

	void Awake () {
		if (radius == 0) {
			radius = GameState.fieldRadius;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if ((transform.position - center).sqrMagnitude > radius * radius) {
//			print ("Boom boom");
			Vector2 vel = GetComponent<Rigidbody2D> ().velocity;
			GetComponent<Rigidbody2D>().velocity = Vector2.Reflect (vel, -transform.position.normalized);
		}
	}
}
