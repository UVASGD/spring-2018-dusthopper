using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

	private Rigidbody2D rb;
	private Vector3 lastPos;
	private float speed = 5f;
	private Transform myAsteroid;
	private bool chasing = false;
	Vector3 targetPosition = Vector3.zero;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		myAsteroid = this.transform.parent;
		//GameState.asteroid = GameObject.FindWithTag ("Hub").transform;
		//transform.position = GameState.asteroid.position;
		// change transform.position to be parent asteroid?
		//transform.position = myAsteroid.position;
		lastPos = transform.localPosition;

		Wander ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {

		float step = speed * Time.deltaTime;

		Debug.Log (Vector2.Distance (transform.localPosition, targetPosition));


		if (!chasing) {
			if (Vector2.Distance (transform.localPosition, targetPosition) > 0.1) {
				transform.localPosition = Vector2.Lerp (transform.localPosition, targetPosition, step);
			} else {
				Wander ();
				Debug.Log ("wandering");
			}

			//Debug.Log ("local = " + transform.localPosition);
			//Debug.Log ("absolute = " + transform.position);

			//Vector3 positionChange = myAsteroid.position - lastPos;

			//transform.localPosition += positionChange;
			//lastPos = myAsteroid.position;


		}


		//Debug.Log (myAsteroid.GetInstanceID() + ": " + positionChange + ", " + myAsteroid.position.ToString() + ", " + transform.localPosition.ToString() + ", " + targetPosition.ToString());
	}

	private void Wander() {
		targetPosition = new Vector2 (Random.Range (-1.0f, 1.0f), Random.Range (-1.0f, 1.0f));

		if (GameState.mapOpen) {
			targetPosition = Vector2.zero;
		}

		//Stop following asteroidmovement if there is none
		if (!myAsteroid) {
			Debug.Log ("no asteroid");
			return;
		}

		//Keep constrained on current asteroid
//		if (!Movement.IsWithinAsteroid(transform, targetPosition, myAsteroid)) {
//			
//			targetPosition = Vector2.zero;
//		}




	}


	// does random movement and stuff
	//
}
