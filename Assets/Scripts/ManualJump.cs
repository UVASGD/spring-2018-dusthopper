using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualJump : MonoBehaviour {
	//Allows the player to charge up a jump by holding down left mouse button and then jumps to the first asteroid in the direction of cursor.
	//TODO: sound effect weirdness
	private float timeHeld = 0f;
	public AudioSource asrc;

	void Start(){
		timeHeld = 0f;
	}
	// Update is called once per frame
	void Update () {
//		print (timeHeld);
		if (!GameState.mapOpen && !GameState.manualJumpsDisabled) {
			if (Input.GetMouseButton (0)) {
				if (timeHeld >= GameState.secondsPerJump) {
					Vector2 directionOfCursor = (Vector2)(Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position);
					int everythingExceptAsteroids = ~LayerMask.NameToLayer ("Asteroid"); //ignore raycasts with everything except asteroids
					RaycastHit2D[] thingsIHit = Physics2D.RaycastAll ((Vector2)transform.position, directionOfCursor, GameState.maxAsteroidDistance, everythingExceptAsteroids);
					if (thingsIHit.Length > 2) {
						Transform otherAsteroid = thingsIHit[2].transform; ////thingsIHit[0] is us and thingsIHit[1] is the asteroid we're standing on so we want the next one
						print(otherAsteroid.gameObject.name);
						GetComponent<Movement> ().SwitchAsteroid (otherAsteroid);
					} else {
						print ("didn't hit anything");
						//TODO "jump" to point in space at end of raycast and die / lose a life
					}
					timeHeld = 0;
					asrc.Stop ();
				} else {
					timeHeld += Time.deltaTime;
					if (!asrc.isPlaying) {
						asrc.Play ();
					}
				}
			} else {
				timeHeld = 0;
				if (asrc.isPlaying) {
					asrc.Stop ();
				}
			}
		}
	}
}
