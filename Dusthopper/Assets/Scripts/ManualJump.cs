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
		if (!GameState.mapOpen && !GameState.manualJumpsDisabled && !GameState.gamePaused) {
			if (Input.GetMouseButton (0)) {
				if (timeHeld >= GameState.secondsPerJump) {
					Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					Vector2 directionOfCursor = (Vector2)(cursorPosition - transform.position);
					int onlyAsteroids = (1 << LayerMask.NameToLayer("Asteroid"));
					RaycastHit2D[] thingsIHit = Physics2D.RaycastAll ((Vector2)transform.position, directionOfCursor, GameState.maxAsteroidDistance, onlyAsteroids);
					if (thingsIHit.Length > 1) {
						Transform otherAsteroid = thingsIHit[1].transform; // thingsIHit[0]  is the asteroid we're standing on so we want the next one
						print(otherAsteroid.gameObject.name);
						GetComponent<Movement> ().SwitchAsteroid (otherAsteroid);
					} else {
						print ("didn't hit anything");
						//TODO "jump" to point in space at end of raycast and die / lose a life
						JumpFail(directionOfCursor.normalized * GameState.maxAsteroidDistance + (Vector2)transform.position);
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

	// If you click and hold and there's no asteroid in your path
	void JumpFail (Vector3 targPos) {
		//print ("Jump fail! Going to point " + targPos);
		//GameState.asteroid = null;
		GameState.manualJumpsDisabled = true;
//		GameState.player.transform.parent = null;
		GameObject target = new GameObject("FailJumpPoint");
		target.transform.position = targPos;
		GetComponent<Movement> ().SwitchAsteroid (target.transform, false);
		//print ("WHAT");
		GetComponent<Death> ().Die ();
	}
}
