using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour {

	private bool upgradeShopActivated = false;

	// Update is called once per frame
	void OnGUI() {

		if (!upgradeShopActivated) {
			Rect myRect = new Rect (Screen.width - 130, Screen.height - 80, 120, 30);

			if (GUI.Button (myRect, "Buy Upgrades: " + GameState.scrap)) {
				ActivateUpgradeShop ();
			}
		} else {
			if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 200, 120, 30), "Max Hunger")) {
				GameState.UpgradeMaxHunger ();
				CompletePurchase ();
			}
			if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 160, 120, 30), "Jump Distance")) {
				GameState.UpgradeMaxAsteroidDistance ();
				CompletePurchase ();
			}
			if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 120, 120, 30), "Jump Time")) {
				GameState.UpgradeSecondsPerJump ();
				CompletePurchase ();
			}
			if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 80, 120, 30), "Speed")) {
				GameState.UpgradePlayerSpeed ();
				CompletePurchase ();
			}
			if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 40, 120, 30), "Cancel")) {
				upgradeShopActivated = false;
			}
		}
	}

	private void ActivateUpgradeShop() {
		if (GameState.scrap > 0) {
			upgradeShopActivated = true;
		}
	}

	private void CompletePurchase() {
		GameState.scrap -= 1;
		upgradeShopActivated = false;
	}
}
