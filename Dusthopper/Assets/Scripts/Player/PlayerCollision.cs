﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class isPausedExample {
    [MenuItem("Examples/Scene paused in play mode")]
    static void EditorPlaying() {
        if (EditorApplication.isPaused) {
            Debug.Log("Paused");
        }
    }
}

public class PlayerCollision : MonoBehaviour {

	//This script handles every interaction between the player and an object on the ground

	private Hunger hunger;
	public AudioSource nom;
	public AudioSource chaching; //scrap pickup noise
	public AudioSource GetPlant; //getting plant reward noise

	public bool holding; //Whether or not you are holding something
	public GameObject heldObject; //The object being held.
	GameObject justDroppedObj;  //The object that was recently held
	private float timeSinceDrop = 0.0f; //Used to prevent immediately picking up the same object you dropped.
	public GameObject heldObjLoc; //empty gameobject attached to player

	public float grayPollenFactor = 0.5f;

	public float bluePlantFactor = 0.5f;
	public float bluePlantTimer = 0f;
	public bool onBluePlant = false;
	public bool blueTimerStarted = false;
	public PathMaker myPM;

	void Start(){
		hunger = gameObject.GetComponent<Hunger> ();
		holding = false;
		myPM = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PathMaker> ();
	}

	void Update () {
//		print ("Tutorial Completed: " + GameState.tutorialCompleted);
		if (Input.GetMouseButtonDown (1) && holding) {
			drop ();
		}
		if (timeSinceDrop > 0.0f) {
			timeSinceDrop -= GameState.deltaTime;
			if (timeSinceDrop <= 0.0f) {
				timeSinceDrop = 0.0f;
				justDroppedObj = null;
			}
		}

		if (bluePlantTimer > 0f) {
			if (myPM.path.Count == 0 || blueTimerStarted) {
				
				bluePlantTimer -= GameState.deltaTime;
				if (!blueTimerStarted) {
					GameObject.Find ("GetPlantSFX").GetComponent<AudioSource> ().Play ();
					blueTimerStarted = true;
				}
//				Debug.Log (bluePlantTimer);
				if (!onBluePlant) {
					Debug.Log ("blue on");

					onBluePlant = true;
					GameState.secondsPerJump = GameState.secondsPerJump * bluePlantFactor;
				}
			}
		} else if (onBluePlant) {
			Debug.Log ("blue off");
//			Debug.Log (bluePlantTimer);
			onBluePlant = false;
			blueTimerStarted = false;
			GameState.secondsPerJump = GameState.secondsPerJump / bluePlantFactor;
		}
	}

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Food") {
 //           print("Collided with food (stay)");
            Eat(other.gameObject);
        }

    }

    //Add cases for what to do with certain objects here
    void OnTriggerEnter2D ( Collider2D other){
		if (other.gameObject.tag == "Food"){
//  			print ("Collided with food (enter)");
			Eat (other.gameObject);
		}

		if (other.tag == "Scrap") {
			//print ("collided with scrap");
			GameState.scrap += other.gameObject.GetComponent<ScrapBehavior> ().scrapValue;
            chaching.Play(); //play sound effect
			Destroy (other.gameObject);
		}
		if (other.tag == "Pollen" && !holding && other.gameObject != justDroppedObj) {
//			print ("Picked up pollen");
			heldObject = other.gameObject;
			holding = true;
			other.transform.SetParent(gameObject.transform);
			other.transform.position = heldObjLoc.transform.position;
			if (other.name.ToLower().Contains("gray")) {
				// Limit jump distance
				GameState.maxAsteroidDistance = grayPollenFactor*GameState.maxAsteroidDistance;

			}
		}

		if (other.tag == "Plant" && holding) {
            if (heldObject.GetComponent<Pollen> () != null && !other.GetComponent<Plant>().bloomed) {
				if (heldObject.GetComponent<Pollen> ().name == other.GetComponent<Plant> ().myPollen) {
					Debug.Log ("you gave the plant some pollen!");
					if (heldObject.name.ToLower().Contains("red")) {
						resetJumpDistance();
					}


					other.GetComponent<Plant> ().Bloom (heldObject);
					if (GetPlant) {
						GetPlant.Play ();
					}

					Destroy (heldObject);
					holding = false;
				}
			}
		}
	}

	void Eat(GameObject food){
//        print("in eat method");
        Food thisObject = food.GetComponent<Food>();

        if (thisObject.canEat()) {
 //           print("determined can eat");
            hunger.addToHunger (food.GetComponent<Food> ().hungerUp);
		    nom.Play ();
		    Destroy (food);
        }

        
	}

	void drop() {
		//print ("Dropped pollen");
		holding = false;
		heldObject.transform.parent = GameState.asteroid;
		justDroppedObj = heldObject;
		if (heldObject.name.ToLower().Contains("gray")) {
			resetJumpDistance();
		}
		heldObject = null;
		timeSinceDrop = 0.5f;
	}

	void resetJumpDistance() {
        GameState.maxAsteroidDistance = GameState.maxAsteroidDistance;
	}

	public void setBlueTimer(float myTime) {
		bluePlantTimer = myTime;		
	}

}
