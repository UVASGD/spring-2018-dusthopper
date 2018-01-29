using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ryan Kann
// 
// Description
// Animate the player as he jumps. Currently, disables the renderer on the actual player, makes a sprite clone, and directly
// lerps the clone position from the current asteroid to the destination asteroid. Pretty crude, and definitely noticable, but
// works for now. In the future, should find a better, smoother system.

public class JumpAnimationOLD : MonoBehaviour {

	public Transform origin;
	public Transform destination;

	public float animationSpeed = 2;
	private float smoothing;

	private Vector3 vel;

	void Start () {
		smoothing = 0.15f;
		GameObject.FindWithTag ("Player").GetComponent<SpriteRenderer> ().enabled = false;
	}

	void Update () {
		if (origin) {
			transform.position = Vector3.SmoothDamp (transform.position, origin.position + 2.5f * Time.deltaTime * (Vector3)GameState.asteroid.GetComponent<Rigidbody2D> ().velocity, ref vel, smoothing);
			if ((transform.position - origin.position).magnitude < 0.5f) {
				smoothing = 0.05f;
				if ((transform.position - origin.position).magnitude < 0.2f) {
					GameObject.FindWithTag ("Player").GetComponent<SpriteRenderer> ().enabled = true;
					Destroy (gameObject);
				}
			}
		}
	}

	// Update is called once per frame
	public void Animate () {
		float startTime = Time.time;
		float lerpVal = 0;
		int overflow = 0;
		//print ("Origin: " + origin + "\tDest: " + destination + "\tDist: " + (origin.position - destination.position).magnitude);
		//print ("Start time: " + startTime);

		if (origin != null && destination != null) {
			//print (lerpVal);
			while (lerpVal < 1 && overflow++ < 100) {
				//print (overflow);
				//print ("Time: " + Time.time + "\tstartTime: " + startTime);
				lerpVal += Time.deltaTime * animationSpeed;
				transform.position = Vector3.Lerp (origin.position, destination.position, lerpVal);

			}
			print (overflow);
			print (lerpVal);
			if (lerpVal >= 1) {
				Destroy (gameObject, 5f);
			}
		}
	}
}
