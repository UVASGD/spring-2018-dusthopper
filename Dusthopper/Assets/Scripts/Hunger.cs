using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hunger : MonoBehaviour {
	//Does a hunger bar incl. GUI and has methods relating to hunger
	//private float hunger;
	public Color fullHungerBarColor;
	public Color emptyHungerBarColor;
	public Color currentHungerBarColor;
	public bool debugDontLoseHunger;
	private GUIStyle gstyle;
	//public float maxHunger;
	private int hungerBarWidth;
	private int hungerBarHeight = 25;
	public bool changeColor = true;
	public Slider hungerSlider;
	public Image HungerSliderColor;

	// Use this for initialization
	void Start () {
		GameState.hunger = GameState.maxHunger;
		hungerBarWidth = Screen.width * 4 / 8;
		debugDontLoseHunger = false;
		currentHungerBarColor = fullHungerBarColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.hunger <= 0) {
			GameState.hunger = 0;
			gameObject.GetComponent<Death> ().Die ();
		} else {
			if (!debugDontLoseHunger && GameState.hungerEnabled) {
				GameState.hunger -= (GameState.deltaTime / GameState.hungerLowModifier);
			}
			//print ("hunger: " + hunger);
			if (changeColor) {
				currentHungerBarColor = Color.Lerp (emptyHungerBarColor, fullHungerBarColor, GameState.hunger / GameState.maxHunger);
				HungerSliderColor.color = currentHungerBarColor;
			}
			hungerSlider.value = (GameState.hunger / GameState.maxHunger);
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
