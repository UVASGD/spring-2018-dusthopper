using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finished, but need to abstract to other things if anything other than player jumps
public class JumpAnimation : MonoBehaviour {

	[HideInInspector]
	public Transform origin;
	[HideInInspector]
	public Transform destination;
	private Transform animationChild;
	private Vector3 childRelativePos;

	private AudioSource chaching;
	public float animationSpeed = 2;
	private float smoothing;

	private Vector3 vel;

	void Start () {
		
		smoothing = 0.15f;
		GameState.player.GetComponent<SpriteRenderer> ().enabled = false;
		chaching = GameState.player.GetComponent<PlayerCollision> ().chaching;
		if (GameState.player.GetComponent<PlayerCollision> ().holding) {
			GameState.player.GetComponent<PlayerCollision>().heldObject.SetActive(false);
		}
//		Vector3 heading = (destination.transform.position - origin.transform.position);
//		float angle = Mathf.Atan2 (heading.y, heading.x) * Mathf.Rad2Deg;
//		transform.eulerAngles = Vector3.forward * (angle + 90);
	}

	void Update () {
		if (origin) {
			Vector3 directVect = (origin.position + 2.5f * Time.deltaTime * (Vector3)GameState.asteroid.GetComponent<Rigidbody2D> ().velocity - transform.position).normalized;
			float angle = Mathf.Atan2 (directVect.y, directVect.x) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.forward * (angle + 90);

			transform.position = Vector3.SmoothDamp (transform.position, origin.position + 2.5f * Time.deltaTime * (Vector3)GameState.asteroid.GetComponent<Rigidbody2D> ().velocity, ref vel, smoothing);
			if ((transform.position - origin.position).magnitude < 0.5f) {
				smoothing = 0.05f;

				/*
				if (GameState.player.transform.childCount > 0) {
					print ("SHOOP");
					animationChild = GameState.player.transform.GetChild (0);
					childRelativePos = animationChild.localPosition;
					animationChild.SetParent (transform);
					animationChild.localPosition = childRelativePos;
					animationChild.GetComponent<Collider2D> ().enabled = false;
				}
				*/

				if ((transform.position - origin.position).magnitude < 0.2f) {
					/*
					if (animationChild) {
						animationChild.SetParent (GameState.player.transform);
						animationChild.GetComponent<Collider2D> ().enabled = true;
						animationChild.localPosition = childRelativePos;
					}*/

//					print ("Destination Reached");
					GameState.player.GetComponent<SpriteRenderer> ().enabled = true;
					if (GameState.player.GetComponent<PlayerCollision> ().holding) {
						GameState.player.GetComponent<PlayerCollision> ().heldObject.SetActive (true);
					}

                    AsteroidInfo aI = destination.GetComponent<AsteroidInfo>();
                    if (aI != null) aI.TriggerPulse();
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

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "ScrapInCloud") {
            print("jump collided with scrap");
            GameState.scrap += collision.gameObject.GetComponent<ScrapBehavior>().scrapValue;
            chaching.Play(); //play sound effect
            Destroy(collision.gameObject);
        }
    }
}
