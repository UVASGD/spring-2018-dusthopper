using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finished for now
public class UpdateTime : MonoBehaviour {
	//This script's job is to keep GameState.time and GameState.deltaTime synced with Time.time and Time.deltaTime, except for time spent fast-forwarding in the map.


	//there's one frame at the beginning of opening the map where mapOpen has been set to true but Time.timeScale still hasn't been set to zero. 
	//I want to count the deltaTime from this frame but not the ones where fastforwarding is taking place
	private bool weirdFrameHappened;

	void Start(){
		weirdFrameHappened = false;
		GameState.time = -Time.deltaTime;
	}

	void Update () {
		if (GameState.mapOpen) {
			if (!weirdFrameHappened) {
				GameState.deltaTime = Time.deltaTime;
				GameState.time += Time.deltaTime;
				GetComponent<PathMaker> ().initialTime = GameState.time;
				weirdFrameHappened = true;
			} else {
				GameState.deltaTime = 0f;
			}
		} else {
			GameState.deltaTime = Time.deltaTime;
			GameState.time += Time.deltaTime;
			weirdFrameHappened = false;
		}
	}
}
