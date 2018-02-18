using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	void OnGUI()
	{
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
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}
}
