using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour {

	private bool upgradeShopActivated = false;

	// Number of times upgraded (for calculating cost)
	float costMaxHunger = 1f;
	float costJumpDistance = 1f;
	float costJumpTime = 1f;
	float costSpeed = 1f;

	// Update is called once per frame
	void OnGUI() {

		if (GameState.asteroid.tag == "Hub") {
			if (!upgradeShopActivated) {
				Rect myRect = new Rect (Screen.width - 130, Screen.height - 80, 120, 30);

				if (GUI.Button (myRect, "Buy Upgrades: " + GameState.scrap)) {
					ActivateUpgradeShop ();
				}
			} else {
				if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 200, 120, 30), (int)costMaxHunger + ": Max Hunger")) {
					if (GameState.scrap >= (int)costMaxHunger) {
						GameState.UpgradeMaxHunger ();
						CompletePurchase (costMaxHunger);
						costMaxHunger *= 1.4f;
					}
				}
				if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 160, 120, 30), (int)costJumpDistance + ": Jump Distance")) {
					if (GameState.scrap >= (int)costJumpDistance) {
						GameState.UpgradeMaxAsteroidDistance ();
						CompletePurchase (costJumpDistance);
						costJumpDistance *= 1.4f;
					}
				}
				if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 120, 120, 30), (int)costJumpTime + ": Jump Time")) {
					if (GameState.scrap >= (int)costJumpTime) {
						GameState.UpgradeSecondsPerJump ();
						CompletePurchase (costJumpTime);
						costJumpTime *= 1.4f;
					}
				}
				if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 80, 120, 30), (int)costSpeed + ": Speed")) {
					if (GameState.scrap >= (int)costSpeed) {
						GameState.UpgradePlayerSpeed ();
						CompletePurchase (costSpeed);
						costSpeed *= 1.4f;
					}
				}
				if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 40, 120, 30), "Cancel")) {
					upgradeShopActivated = false;
				}
			}
		}
	}

	private void ActivateUpgradeShop() {
		if (GameState.scrap > 0) {
			upgradeShopActivated = true;
		}
	}

	private void CompletePurchase(float cost) {
		GameState.scrap -= (int)cost;
		GameState.SaveGame ();
		upgradeShopActivated = false;
	}
}
