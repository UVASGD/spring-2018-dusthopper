using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBoi : Animal {


	protected override void Update() {


		bool playerIsOnAsteroid = GameState.asteroid == myAsteroid;

		// If player is on this asteroid, follow player
		if (playerIsOnAsteroid) {
			Chase ();
		} else { // wander
			wandering = true;
			base.Update ();
		}
	}

	private void Chase() {
		wandering = false;
		targetPosition = (GameState.player.transform.position - GameState.asteroid.transform.position - transform.localPosition);
		base.Update ();
	}

}
