using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour {
	//Does a hunger bar incl. GUI and has methods relating to hunger
	private float hunger;
	public Color hungerBarColor; //TODO: Change display to show a colored rectangle. also make red / yellow for when close to death
	private float hungerBarWidth;
	private bool debugDontLoseHunger;
	private GUIStyle gstyle;
	public float maxHunger;
	// Use this for initialization
	void Start () {
		hunger = maxHunger;
		hungerBarWidth = Screen.width * 5 / 8;
		debugDontLoseHunger = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (hunger <= 0) {
			hunger = 0;
			gameObject.GetComponent<Death> ().Die ();
		} else {
			if (!debugDontLoseHunger) {
				hunger -= GameState.deltaTime;
			}
			//print ("hunger: " + hunger);
		}
	}

	void OnGUI () {
		GUI.Box (new Rect (Screen.width * 1/2 - hungerBarWidth * 0.5f * (hunger / maxHunger), Screen.height * 7/8, hungerBarWidth * (hunger / maxHunger), 50),"");

		//This is just a debug button to toggle hunger loss (in case having to worry about that is annoying for testing)
		if (GUI.Button(new Rect(10, Screen.height - 80, 120, 30), "No hunger")) {
			debugDontLoseHunger = !debugDontLoseHunger;
		}
	}

	public void setHunger(float newHunger){
		hunger = newHunger;
	}

	public void addToHunger(float amount){
		hunger += amount;
		if (hunger > maxHunger) {
			hunger = maxHunger;
		}
	}

	public float getHunger(){
		return hunger;
	}	
}
