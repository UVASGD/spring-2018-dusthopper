using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour {
	//Does a hunger bar incl. GUI and has methods relating to hunger
	//private float hunger;
	public Color fullHungerBarColor; //TODO: Change display to show a colored rectangle. also make red / yellow for when close to death
	public Color emptyHungerBarColor;
	private Color currentHungerBarColor;
	private bool debugDontLoseHunger;
	private GUIStyle gstyle;
	//public float maxHunger;
	private int hungerBarWidth;
	private int hungerBarHeight = 25;
	public bool changeColor = true;

	// Use this for initialization
	void Start () {
		GameState.hunger = GameState.maxHunger;
		hungerBarWidth = Screen.width * 4 / 8;
		debugDontLoseHunger = false;
		gstyle = new GUIStyle ();
		currentHungerBarColor = fullHungerBarColor;
		gstyle.normal.background = MakeTex((int)(hungerBarWidth + 1),hungerBarHeight,currentHungerBarColor);
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.hunger <= 0) {
			GameState.hunger = 0;
			gameObject.GetComponent<Death> ().Die ();
		} else {
			if (!debugDontLoseHunger && GameState.hungerEnabled) {
				GameState.hunger -= GameState.deltaTime;
			}
			//print ("hunger: " + hunger);
			if (changeColor) {
				currentHungerBarColor = Color.Lerp (emptyHungerBarColor, fullHungerBarColor, GameState.hunger / GameState.maxHunger);
				gstyle.normal.background = MakeTex ((int)(hungerBarWidth + 1), hungerBarHeight, currentHungerBarColor);
			}
		}
	}

	void OnGUI () {
		GUI.Box (new Rect (Screen.width * 4/16, Screen.height * 15/16, hungerBarWidth * (GameState.hunger / GameState.maxHunger), 15), "", gstyle);

		if (!GameState.debugMode)
			return;
		
		//This is just a debug button to toggle hunger loss (in case having to worry about that is annoying for testing)
		if (GUI.Button(new Rect(10, Screen.height - 80, 120, 30), "No hunger")) {
			debugDontLoseHunger = !debugDontLoseHunger;
		}
	}

	public void setHunger(float newHunger){
		GameState.hunger = newHunger;
		if (GameState.hunger >= GameState.maxHunger) {
			GameState.hunger = GameState.maxHunger;
		}

	}

	public void addToHunger(float amount){
		GameState.hunger += amount;
		if (GameState.hunger > GameState.maxHunger) {
			GameState.hunger = GameState.maxHunger;
		}
	}

	public float getHunger(){
		return GameState.hunger;
	}

	private Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for( int i = 0; i < pix.Length; ++i )
		{
			pix[ i ] = col;
		}
		Texture2D result = new Texture2D( width, height );
		result.SetPixels( pix );
		result.Apply();
		return result;
	}
}
