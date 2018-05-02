using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeShop : MonoBehaviour {
	bool openShop;
	public Text useScrapText;
	public Text hungerCostText;
	public Text jumpDistCostText;
	public Text jumpTimeCostText;
	public Text speedCostText;
	public GameObject hungerButton;
	public GameObject jumpDistButton;
	public GameObject jumpTimeButton;
	public GameObject speedButton;
	// Update is called once per frame
	void Update () {
		useScrapText.text = ((int)GameState.scrap).ToString ();
		int[] costsToDisplay = GameState.player.GetComponent<Scrap> ().getCosts ();
		hungerCostText.text = costsToDisplay[0].ToString ();
		jumpDistCostText.text = costsToDisplay[1].ToString ();
		jumpTimeCostText.text = costsToDisplay[2].ToString ();
		speedCostText.text = costsToDisplay[3].ToString ();
//		speedCostText.text = "fuck you";
		//If you cant afford, hide the button
		if ((int)GameState.scrap < costsToDisplay[0]) {
			hungerButton.SetActive (false);
		} else {
			hungerButton.SetActive (true);
		}
		if ((int)GameState.scrap < costsToDisplay[1]) {
			jumpDistButton.SetActive (false);
		} else {
			jumpDistButton.SetActive (true);
		}
		if ((int)GameState.scrap < costsToDisplay[2]) {
			jumpTimeButton.SetActive (false);
		} else {
			jumpTimeButton.SetActive (true);
		}
		if ((int)GameState.scrap < costsToDisplay[3]) {
			speedButton.SetActive (false);
		} else {
			speedButton.SetActive (true);
		}
	}
}
