using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

	//This script handles every interaction between the player and an object on the ground

	private Hunger hunger;
	public AudioSource nom;

	public bool holding; //Whether or not you are holding something
	public GameObject heldObject; //The object being held.
	GameObject justDroppedObj;  //The object that was recently held
	private float timeSinceDrop = 0.0f; //Used to prevent immediately picking up the same object you dropped.

	void Start(){
		hunger = gameObject.GetComponent<Hunger> ();
		holding = false;
	}

	void Update () {
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
	}

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Food") {
            print("EATING FOOD");
            Eat(other.gameObject);
        }

    }

    //Add cases for what to do with certain objects here
    void OnTriggerEnter2D ( Collider2D other){
		if (other.gameObject.tag == "Food"){
			print ("EATING FOOD");
			Eat (other.gameObject);
		}

		if (other.tag == "Pollen" && !holding && other.gameObject != justDroppedObj) {
			print ("Picked up pollen");
			heldObject = other.gameObject;
			holding = true;
			other.transform.SetParent(gameObject.transform);
		}

		if (other.tag == "Plant" && holding) {
			if (heldObject.GetComponent<Pollen> () != null) {
				if (heldObject.GetComponent<Pollen> ().name == other.GetComponent<Plant> ().myPollen) {
					Debug.Log ("you gave the plant some pollen!");
					other.GetComponent<Plant> ().dispenseReward ();
					Destroy (heldObject);
					holding = false;
				}
			}
		}
	}

	void Eat(GameObject food){

        Food thisObject = food.GetComponent<Food>();

        if (thisObject.canEat()) {
            hunger.addToHunger (food.GetComponent<Food> ().hungerUp);
		    nom.Play ();
		    Destroy (food);
        }

        
	}

	void drop() {
		print ("Dropped pollen");
		holding = false;
		heldObject.transform.parent = GameState.asteroid;
		justDroppedObj = heldObject;
		heldObject = null;
		timeSinceDrop = 0.5f;
	}

}
