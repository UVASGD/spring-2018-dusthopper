using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsUIController : MonoBehaviour {

	public Text maxDistText;
	public Text jumpTimeText;
	public Text speedText;
	public Text hungerText;
	public Text maxHungerText;
	public Text scrapText;
	public Text hungerDrip;
	public Text fragmentCount;

	void Update () {
		maxDistText.text = GameState.maxAsteroidDistance.ToString ("N1");
		jumpTimeText.text = GameState.secondsPerJump.ToString ("N1");
		speedText.text = GameState.playerSpeed.ToString ("N1");
		hungerText.text = GameState.player.GetComponent<Hunger>().getHunger().ToString ("N1");
		maxHungerText.text = GameState.maxHunger.ToString ("N1");
		scrapText.text = GameState.scrap.ToString ("N1");
		hungerDrip.text = (Mathf.Floor(Time.deltaTime / GameState.hungerLowModifier * 1000)).ToString("N0");
		fragmentCount.text = GameState.gravityFragmentCount.ToString();
	}
}
