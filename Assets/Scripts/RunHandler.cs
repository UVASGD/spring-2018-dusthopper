using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Call this when starting a run. More or less randomizes asteroid belt
	public void StartRun () {

	}

	//Call this any time a run ends, whether due to death or returning to hub
	public void EndRun (bool successful) {
		if (successful) {
			GameState.SaveGame ();
		} else {
					
		}
	}
}
