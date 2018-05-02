using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scrap : MonoBehaviour {

	[SerializeField] private bool upgradeShopActivated = false;

	// Number of times upgraded (for calculating cost)
	float costMaxHunger;
	float costJumpDistance;
	float costJumpTime;
    float costSpeed;

//	public Text hungerCostText;
//	public Text jumpDistCostText;
//	public Text jumpTimeCostText;
//	public Text speedCostText;

	float percentIncreaseMaxHunger = 0.05f;
	float percentIncreaseJumpDistance = 0.10f;
	float percentIncreaseJumpTime = 0.05f;
	float percentIncreaseSpeed = 0.10f;

	public GameObject shop;
	bool openShop;

	void Start () {
		costMaxHunger = Mathf.Max(1, Mathf.Log (GameState.maxHunger / GameState.defaultMaxHunger, 1 + percentIncreaseMaxHunger));
		costJumpDistance = Mathf.Max(1, Mathf.Log (GameState.savedMaxAsteroidDistance / GameState.defaultMaxAsteroidDistance, 1 + percentIncreaseJumpDistance));
		costJumpTime = Mathf.Max(1, Mathf.Log (GameState.defaultSecondsPerJump / GameState.savedSecondsPerJump, 1 + percentIncreaseJumpTime));
		costSpeed = Mathf.Max(1, Mathf.Log (GameState.playerSpeed / GameState.defaultPlayerSpeed, 1 + percentIncreaseSpeed));

//
//		hungerCostText.text = ((int)costMaxHunger).ToString ();
//		print (jumpDistCostText.text);
//		jumpDistCostText.text = ((int)costJumpDistance).ToString ();
//		print (jumpDistCostText.text);
//		jumpTimeCostText.text = ((int)costJumpTime).ToString ();
//		speedCostText.text = ((int)costJumpTime).ToString ();
		openShop = false;
	}

//	void Update() {
//		//costMaxHunger += Time.deltaTime;
//		//print ("in Scrap cost = " + GameState.player.GetComponent<Scrap> ().costJumpTime);
//
//		//Pressing "i" will toggle shop if your in the hub
//		if (GameState.asteroid.tag == "Hub" && Input.GetKeyDown (KeyCode.I)) {
//			openShop = !openShop;
//			if (openShop) {
//				shop.SetActive (true);
//			} else {
//				shop.SetActive (false);
//			}
//		}
//	}
		
	// Update is called once per frame
	void OnGUI() {
		if (GameState.asteroid == null) {
			return;
		}

		if (GameState.asteroid.tag == "Hub") {
			if (!upgradeShopActivated) {
				Rect myRect = new Rect (Screen.width - 130, Screen.height - 80, 120, 30);

				if (GUI.Button (myRect, "Buy Upgrades: " + GameState.scrap)) {
					ActivateUpgradeShop ();

				}
			} 

			else {
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
		print ("Cost: " + cost);
		GameState.scrap -= (int)cost;
		print ("Scrap: " + GameState.scrap);
		GameState.SaveGame ();
		upgradeShopActivated = false;
	}

	public void buyHunger() {
		GameState.UpgradeMaxHunger ();
		print ("CostMaxHunger: " + costMaxHunger);
		CompletePurchase (costMaxHunger);
		costMaxHunger *= 1.4f;
		//print ("Current Scrap: " + GameState.scrap);
//		hungerCostText.text = ((int)costMaxHunger).ToString ();

	}

	public void buyJumpDistance() {
		GameState.UpgradeMaxAsteroidDistance ();
		CompletePurchase (costJumpDistance);
		costJumpDistance *= 1.4f;
//		jumpDistCostText.text = ((int)costJumpDistance).ToString ();
	}

	public void buyJumpTime() {
		GameState.UpgradeSecondsPerJump ();
		CompletePurchase (costJumpTime);
		costJumpTime *= 1.4f;
//		print (jumpTimeCostText.text);
//		jumpTimeCostText.text = ((int)costJumpTime).ToString ();
	}

	public void buySpeed() {
		GameState.UpgradePlayerSpeed ();
		CompletePurchase (costSpeed);
		costSpeed *= 1.4f;
//		speedCostText.text = ((int)costJumpTime).ToString ();
	}

	public int[] getCosts(){
		int[] costs = {(int)costMaxHunger,(int)costJumpDistance,(int)costJumpTime,(int)costSpeed};
		return costs;
	}
}

