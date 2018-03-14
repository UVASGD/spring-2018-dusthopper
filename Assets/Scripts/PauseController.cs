using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {

	private float prevTimeScale = 1f;

	public GameObject pauseMenuUI;

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
		pauseMenuUI.SetActive (false);
		Time.timeScale = prevTimeScale;
	}

	public void Pause(){
		print ("pause called");
		GameState.gamePaused = true;
		pauseMenuUI.SetActive (true);
		prevTimeScale = Time.timeScale;
		Time.timeScale = 0f;
	}
}
