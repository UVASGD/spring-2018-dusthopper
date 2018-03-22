using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

	private Rigidbody2D rb;
	private Vector3 lastPos;
	private float speed = 5;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		//GameState.asteroid = GameObject.FindWithTag ("Hub").transform;
		//transform.position = GameState.asteroid.position;
		// change transform.position to be parent asteroid?
		lastPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// pick a point to randomly move to



		Vector2 targVel = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized * (speed * upgradeMgr.walkSpeedMod);

		if (GameState.mapOpen || (Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0)) {
			targVel = Vector3.zero;
		}

		//Stop following asteroid movement if there is none
		if (!GameState.asteroid)
			return;

		//Keep constrained on current asteroidj
		if ((((Vector2)transform.position + targVel * Time.deltaTime) - (Vector2)GameState.asteroid.position + GameState.asteroid.GetComponent<Rigidbody2D>().velocity * Time.deltaTime).magnitude < GameState.asteroid.GetComponent<AsteroidInfo>().radius) {
			rb.velocity = targVel;
		} else {
			rb.velocity = Vector2.zero;
		}

		transform.position += GameState.asteroid.position - lastPos;
		lastPos = GameState.asteroid.position;
	}


	// does random movement and stuff
	//
}
