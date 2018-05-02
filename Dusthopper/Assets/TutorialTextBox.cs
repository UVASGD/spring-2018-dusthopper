using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextBox : MonoBehaviour {

	[TextArea(1, 2)] public string text = "";
	public Requirement requirement;
	public bool canMove;
	public bool canZoom;
	public bool canJump;
	public bool canPlanJump;

	// Use this for initialization
	void Awake () {
		transform.GetChild (0).GetChild (0).GetComponent<Text> ().fontSize = 17;
		if (text != "") {
			transform.GetChild(0).GetChild(0).GetComponent<Text> ().text = text;
		}
		if (text.Length >= 75) {
			transform.GetChild(0).GetChild(0).GetComponent<Text> ().fontSize -= 2;
		}

		if (requirement != Requirement.none) {
			transform.GetChild (1).gameObject.SetActive (false);
		}
	}
}

public enum Requirement {
	none,
	move,
	manualJump,
	openMap,
	planJump,
	closeMap,
	eatFood,
	collectScrap,
	pickupSeed,
	depositSeed,
	returnToHub
}
