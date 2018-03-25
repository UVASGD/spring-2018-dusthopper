using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {

	private float prevTimeScale = 1f;

	public GameObject pauseMenuUI;
	public GameObject settingsMenuUI;

	private bool inSettings;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (!GameState.gamePaused) {
				Pause ();
			} else {
				Resume ();
			}
		}
//		print ("Time.timeScale: " + Time.timeScale);
	}

	public void Resume(){
		GameState.gamePaused = false;
		inSettings = false;
		pauseMenuUI.SetActive (false);
		settingsMenuUI.SetActive (false);
		Time.timeScale = prevTimeScale;
	}

	public void Pause(){
		print ("pause called");
		GameState.gamePaused = true;
		pauseMenuUI.SetActive (true);
		prevTimeScale = Time.timeScale;
		Time.timeScale = 0f;
	}

	public void GoToSettings(){
		print ("going to settings");
		pauseMenuUI.SetActive (false);
		settingsMenuUI.SetActive (true);
		inSettings = true;

	}

	public void LeaveSettings(){
		print ("leaving settings");
		inSettings = false;
		pauseMenuUI.SetActive (true);
		settingsMenuUI.SetActive (false);
	}
}
