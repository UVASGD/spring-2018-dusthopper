using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunHandler : MonoBehaviour {

	public bool onHub;
	public bool onHubLF;

	// Use this for initialization
	void Awake () {
		onHubLF = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.asteroid.tag == "Hub") {
			onHub = true;

			if (!onHubLF) {
				EndRun (true);
			}
		} else {
			onHub = false;

			if (onHubLF) {

			}
		}
		onHubLF = onHub;
	}

	//Call this when starting a run. More or less randomizes asteroid belt
	public void StartRun () {
		
	}

	//Call this any time a run ends, whether due to death or returning to hub
	public void EndRun (bool successful) {
		if (successful) {
			print ("Successfully completed run!");
			GameState.SaveGame ();
		} else {
			print ("Everything obtained during your run has been deleted.");
			GameState.LoadGame ();
		}
	}
}
