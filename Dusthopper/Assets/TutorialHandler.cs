using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour {
	int tutorialStage = 0;
	bool conditionMet = false;
	bool canMove = false;
	Requirement currentRequirement;
	float timeSinceLastStage = 0f;
	float timeSpentMoving = 0f;

	private Movement playerMove;

	// Use this for initialization
	void Start () {
		tutorialStage = 0;
		conditionMet = false;
		timeSinceLastStage = 0f;
		timeSpentMoving = 0f;
		canMove = false;

		currentRequirement = Requirement.none;

		playerMove = GameState.player.GetComponent<Movement> ();

		int index = 0;
		foreach (Transform child in transform) {
			if (index++ > 0) {
				child.gameObject.SetActive (false);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		print ("Requirement: " + currentRequirement);
		canMove = false;
		conditionMet = false;
		switch (currentRequirement) {
		default:
			Debug.LogError ("Requirement Not Valid!");
			break;
		case Requirement.none:
			conditionMet = Input.GetKeyDown (KeyCode.N);
			break;
		case Requirement.move:
			canMove = true;
			if (Input.GetAxisRaw ("Horizontal") != 0 || Input.GetAxisRaw ("Vertical") != 0) {
				timeSpentMoving += Time.unscaledDeltaTime;
			}

			if (timeSpentMoving >= 2f) {
				conditionMet = true;
			}
			break;
		case Requirement.manualJump:

			break;
		case Requirement.openMap:

			break;
		case Requirement.planJump:

			break;
		case Requirement.closeMap:

			break;
		case Requirement.eatFood:

			break;
		case Requirement.collectScrap:

			break;
		case Requirement.pickupSeed:

			break;
		case Requirement.depositSeed:

			break;
		case Requirement.returnToHub:

			break;
		}

		timeSinceLastStage += Time.unscaledDeltaTime;

		playerMove.canMove = canMove;

		if (conditionMet) {
			NextStage ();
		}
	}

	public void NextStage () {
		Transform currentChild = transform.GetChild (tutorialStage++);
		Transform nextChild = transform.GetChild (tutorialStage);
		if (currentChild) {
			currentChild.gameObject.SetActive (false);
		}
		if (nextChild) {
			nextChild.gameObject.SetActive (true);
			currentRequirement = nextChild.GetComponent<TutorialTextBox> ().requirement;
		} else {
			EndTutorial ();
		}

		timeSinceLastStage = 0f;
	}

	public void EndTutorial () {
		GameState.ResetGame ();
		GameState.tutorialCompleted = true;
		GameState.SaveGame ();

		Invoke ("FadeOut", 0f);
		Invoke ("StartGame", 5f);
	}

	void FadeOut () {
		FindObjectOfType<FadeController> ().fadeOut (0.2f);
	}

	void StartGame () {
		SceneManager.LoadScene ("MainMenu");
	}
}
