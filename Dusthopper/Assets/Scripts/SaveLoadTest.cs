﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void FixedUpdate () {
		/*if (Input.GetKeyDown (KeyCode.M)) {
			print ("Saving!");
			GameState.SaveGame ();
		} else if (Input.GetKeyDown (KeyCode.N)) {
			print ("Loading!");
			GameState.LoadGame ();
		} else if (Input.GetKeyDown (KeyCode.R)) {
			GameState.maxAsteroidDistance = Random.Range (20f, 30f);
			GameState.secondsPerJump = Random.Range (1f, 5f);
			GameState.playerSpeed = Random.Range (0.2f, 2f);
			GameState.maxHunger = Random.Range (20f, 50f);
		}*/ //hate this...
	}

	void OnGUI()
	{
		if (!GameState.debugMode)
			return;
		
		if(GUI.Button(new Rect(20,20,50,20), "Save"))
		{
			//Debug.Log ("Saving!");
			GameState.SaveGame ();
		}
		
		if(GUI.Button(new Rect(20,50,50,20), "Load"))
		{
			//Debug.Log ("Loading!");
			GameState.LoadGame ();
		}

		if(GUI.Button(new Rect(20,80,50,20), "Reset"))
		{
			//Debug.Log ("Loading!");
			GameState.ResetGame ();
		}

		if(GUI.Button(new Rect(20,110,50,20), "Rand"))
		{
			//Debug.Log ("Loading!");
			GameState.RandomizeStats ();
		}
	}
}