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
					Vector2 directionOfCursor = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					directionOfCursor -= (Vector2)transform.position;
					print (directionOfCursor);
					RaycastHit2D[] thingsIHit = Physics2D.RaycastAll ((Vector2)transform.position, directionOfCursor, GameState.maxAsteroidDistance);
					if (thingsIHit.Length > 1) {
						Transform otherAsteroid = thingsIHit[1].transform;
//						print (otherAsteroid.gameObject.name);
						GetComponent<Movement> ().SwitchAsteroid (otherAsteroid);
					} else {
//						print ("didn't hit anything");
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
