using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

	private Rigidbody2D rb;
	private Vector3 lastPos;
	private float speed = 5;
	private Transform myAsteroid;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		myAsteroid = this.transform.parent;
		//GameState.asteroid = GameObject.FindWithTag ("Hub").transform;
		//transform.position = GameState.asteroid.position;
		// change transform.position to be parent asteroid?
		transform.position = myAsteroid.position;
		lastPos = transform.position;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		// pick a point to randomly move to
		//Vector2 targVel = new Vector2 (lastPos.x + 2, lastPos.y + 2);

		// TODO: make some random movement here
		//Vector2 targVel = RandomVelocity;

		if (GameState.mapOpen || (Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0)) {
			targVel = Vector3.zero;
		}

		//Stop following asteroid movement if there is none
		if (!myAsteroid)
			return;

		//Keep constrained on current asteroidj
		if ((((Vector2)transform.position + targVel * Time.deltaTime) - (Vector2)myAsteroid.position + myAsteroid.GetComponent<Rigidbody2D>().velocity * Time.deltaTime).magnitude < myAsteroid.GetComponent<AsteroidInfo>().radius) {
			rb.velocity = targVel;
		} else {
			rb.velocity = Vector2.zero;
		}

		// TODO fix that
		transform.position += myAsteroid.position - lastPos;
		lastPos = myAsteroid.position;
	}


	// does random movement and stuff
	//
}
