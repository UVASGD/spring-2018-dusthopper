using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

	private Vector3 lastPos;
	private float speed = 1f;
	private float rotationSpeed = 0.1f;
	private Transform myAsteroid;
	private bool chasing = false;
	Vector2 targetPosition = Vector2.zero;
	private Vector2 targRotDir;

	// Use this for initialization
	void Start () {
		myAsteroid = this.transform.parent;
		//GameState.asteroid = GameObject.FindWithTag ("Hub").transform;
		//transform.position = GameState.asteroid.position;
		// change transform.position to be parent asteroid?
		//transform.position = myAsteroid.position;
		lastPos = transform.localPosition;

		float asteroidRadius = myAsteroid.GetComponent<AsteroidInfo> ().radius;

		if (transform.localPosition.magnitude < asteroidRadius) {
			
			transform.localPosition = Vector3.zero;
		}


		Wander ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {


		//animal translational velocity vector
		Vector2 targVel = targetPosition * speed; 

		//This section handles rotation lerping
		//Works by slowing moving point to look at around in unit circle around animal
		targRotDir += targetPosition * rotationSpeed * Time.deltaTime;
		targRotDir = Vector2.ClampMagnitude (targRotDir, 1f);
		float targRot = Mathf.Atan2 (targRotDir.x, -targRotDir.y) * Mathf.Rad2Deg;

		//If the animal isn't moving or the map is open, stop all movement, otherwise move appropriately
		if (GameState.mapOpen || targetPosition == Vector2.zero) {
			targVel = Vector2.zero;
		} else {
			//rb.MoveRotation(targRot); //Directly set animal rotation to the appropriate angle (this is a hard set, the lerping happens in targRot)
			transform.rotation = Quaternion.AngleAxis(targRot,Vector3.forward);
		}

		//Stop following asteroid movement if there is none
		if (!myAsteroid) {
			return;
		}

		//Keep constrained on current asteroid
		if (Vector2.Distance (transform.localPosition, targetPosition) > 0.1) {
			if (IsWithinAsteroid(transform, targVel, myAsteroid)) {
				transform.localPosition += new Vector3(targVel.x*Time.deltaTime,targVel.y*Time.deltaTime,0f);
			} else {
				Wander ();
			}
		} else {
			Wander ();
		}


		float step = speed * Time.deltaTime;
		/*

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


		}*/


		//Debug.Log (myAsteroid.GetInstanceID() + ": " + positionChange + ", " + myAsteroid.position.ToString() + ", " + transform.localPosition.ToString() + ", " + targetPosition.ToString());
	}

	private void Wander() {
		targetPosition = new Vector2 (Random.Range (-0.25f, 0.25f), Random.Range (-0.25f, 0.25f));

		if (GameState.mapOpen) {
			targetPosition = transform.localPosition;
		}

		//Stop following asteroidmovement if there is none
		if (!myAsteroid) {
			return;
		}

		//Keep constrained on current asteroid
		if (!Movement.IsWithinAsteroid(myAsteroid, targetPosition, myAsteroid)) {
			targetPosition = transform.localPosition;
		}




	}


	public static bool IsWithinAsteroid(Transform thisMover, Vector2 targVel, Transform asteroid) {
		Vector2 futureTarget = ((Vector2)thisMover.localPosition + targVel * Time.deltaTime);
		//Vector2 futureAsteroidPosition = (Vector2)asteroid.position + asteroid.GetComponent<Rigidbody2D> ().velocity * Time.deltaTime;
		float asteroidRadius = asteroid.GetComponent<AsteroidInfo> ().radius;

		return futureTarget.magnitude < asteroidRadius;
	}




	// does random movement and stuff
	//
}
