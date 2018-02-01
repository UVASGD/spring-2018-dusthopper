using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ryan Kann
// 
// Description
// Close the game, both in build and in the editor, when the player presses Escape

public class ApplicationManagerOLD : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			QuitGame ();
		}
	}

	void QuitGame () {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
