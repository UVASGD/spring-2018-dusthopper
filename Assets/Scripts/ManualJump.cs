using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualJump : MonoBehaviour {
	private float timeHeld = 0f;

	void Start(){
		timeHeld = 0f;
	}
	// Update is called once per frame
	void Update () {
//		print (timeHeld);
		if (!GameState.mapOpen) {
			if (Input.GetMouseButton (0)) {
				if (timeHeld >= GameState.secondsPerJump) {
					Vector2 directionOfCursor = (Vector2)(Camera.main.ScreenToWorldPoint (Input.mousePosition) - transform.position);
					int everythingExceptAsteroids = ~LayerMask.NameToLayer ("Asteroid"); //ignore raycasts with everything except asteroids
					RaycastHit2D[] thingsIHit = Physics2D.RaycastAll ((Vector2)transform.position, directionOfCursor, GameState.maxAsteroidDistance, everythingExceptAsteroids);
					if (thingsIHit.Length > 1) {
						Transform otherAsteroid = thingsIHit[1].transform; //thingsIHit[0] is the asteroid we're standing on so we want the next one
						GetComponent<Movement> ().SwitchAsteroid (otherAsteroid);
					} else {
						print ("didn't hit anything");
						//TODO "jump" to point in space at end of raycast and die / lose a life
					}
					timeHeld = 0;
				} else {
					timeHeld += Time.deltaTime;
				}
			} else {
				timeHeld = 0;
			}
		}
	}
}
