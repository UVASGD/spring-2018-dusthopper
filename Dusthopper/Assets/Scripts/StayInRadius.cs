using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finished
public class StayInRadius : MonoBehaviour {
	//This script bounces an asteroid off the edge of the world (which is a big circle) keeping it from drifting away
	//Also used by hub and gravity fragment asteroids to keep them in roughly the same world position

	public float radius = 0f;
    public Vector3 center = Vector3.zero;
    float lastTime = 0; //D

	void Awake () {
		if (radius == 0) {
			radius = GameState.fieldRadius;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
        //origninial ((transform.position - center).sqrMagnitude > radius * radius )
        if ((transform.position - center).sqrMagnitude > radius * radius && Mathf.Abs(GameState.time - lastTime) > 2) {
            lastTime = GameState.time; //D
//			print ("Boom boom");
			Vector2 vel = GetComponent<Rigidbody2D> ().velocity;
			GetComponent<Rigidbody2D>().velocity = Vector2.Reflect (vel, -transform.position.normalized);
		}
	}
}
