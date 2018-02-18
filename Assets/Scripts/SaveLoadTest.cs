using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown (KeyCode.M)) {
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
		}
	}
}
