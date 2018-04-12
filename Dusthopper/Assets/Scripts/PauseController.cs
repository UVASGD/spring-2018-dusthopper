using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseController : MonoBehaviour {

	private float prevTimeScale = 1f;

	public GameObject pauseMenuUI;
	public GameObject settingsMenuUI;
	public GameObject debugMenu;
	public GameObject statsDisplay;

	private bool inSettings;

	Action saveDelegate;
	Action loadDelegate;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			if (!GameState.gamePaused) {
				Pause ();
			} else {
				Resume ();
			}
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			if (GameState.debugMode) debugMenu.SetActive(!debugMenu.activeSelf);
			else debugMenu.SetActive(false);
		}
//		print ("Time.timeScale: " + Time.timeScale);
	}

	void Start()
	{
		if (loadDelegate == null) loadDelegate = () => { GameState.LoadGame(); };
		if (saveDelegate == null) saveDelegate = () => { GameState.SaveGame(); };
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

	public void LoadRequest()
	{
		Debug.Log("requesting load");
		if (loadDelegate != null) loadDelegate();
	}

	public void SaveRequest()
	{
		if (saveDelegate != null) saveDelegate();
	}

	public void LeaveSettings(){
		print ("leaving settings");
		inSettings = false;
		pauseMenuUI.SetActive (true);
		settingsMenuUI.SetActive (false);
	}

	public void ToggleStatsDisplay(){
		statsDisplay.SetActive (!statsDisplay.activeSelf);
	}
}
