using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	void Start(){
		hunger = gameObject.GetComponent<Hunger> ();
		holding = false;
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
			bluePlantTimer -= GameState.deltaTime;
//			Debug.Log (bluePlantTimer);
			if (!onBluePlant) {
				Debug.Log ("blue on");

				onBluePlant = true;
				GameState.secondsPerJump = GameState.secondsPerJump * bluePlantFactor;
			}
		} else if (onBluePlant) {
			Debug.Log ("blue off");
//			Debug.Log (bluePlantTimer);
			onBluePlant = false;
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
			if (heldObject.GetComponent<Pollen> () != null) {
				if (heldObject.GetComponent<Pollen> ().name == other.GetComponent<Plant> ().myPollen) {
					Debug.Log ("you gave the plant some pollen!");
					if (heldObject.name.ToLower().Contains("gray")) {
						resetJumpDistance();
					}
					other.GetComponent<Plant> ().dispenseReward ();
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
		GameState.maxAsteroidDistance = GameState.maxAsteroidDistance / grayPollenFactor;
	}

	public void setBlueTimer(float myTime) {
		bluePlantTimer = myTime;		
	}

}
