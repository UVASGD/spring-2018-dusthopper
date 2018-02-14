using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour {
	private float hunger;
	public Color hungerBarColor; //TODO: Change display to show a colored rectangle. also make red / yellow for when close to death
	private GUIStyle gstyle;
	public float maxHunger;
	// Use this for initialization
	void Start () {
		hunger = maxHunger;
	}
	
	// Update is called once per frame
	void Update () {
		if (hunger <= 0) {
			hunger = 0;
			gameObject.GetComponent<Death> ().Die ();
		} else {
			hunger -= GameState.deltaTime;
			//print ("hunger: " + hunger);
		}
	}

	void OnGUI () {
		GUI.Box (new Rect (Screen.width * 1/2 - 200 * (hunger / maxHunger), Screen.height * 7/8, 400 * (hunger / maxHunger), 50),"");
	}

	public void setHunger(float newHunger){
		hunger = newHunger;
	}

	public float getHunger(){
		return hunger;
	}	
}
