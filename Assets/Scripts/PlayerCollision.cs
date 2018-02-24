using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

	//This script handles every interaction between the player and an object on the ground

	private Hunger hunger;
	public AudioSource nom;

	private bool holding; //Whether or not you are holding something
	GameObject heldObject; //The object being held.

	void Start(){
		hunger = gameObject.GetComponent<Hunger> ();
		holding = false;
	}

	void Update () {
		if (Input.GetMouseButtonDown (1) && holding) {
			drop ();
		}
	}


	//Add cases for what to do with certain objects here
	void OnTriggerEnter2D ( Collider2D other){
		if (other.gameObject.tag == "Food"){
			print ("EATING FOOD");
			Eat (other.gameObject);
		}

		if (other.tag == "Seed" && !holding) {
			print ("Picked up a seed");
			heldObject = other.gameObject;
			holding = true;
			other.transform.SetParent(gameObject.transform);
		}

		if (other.tag == "Plant" && holding) {
			if (heldObject.GetComponent<Seed> ().name == other.GetComponent<Plant> ().mySeed) {
				Debug.Log ("for some reason, you gave the plant a seed!");
				Destroy (other.gameObject);
				Destroy (heldObject);
			}
		}
	}

	void Eat(GameObject food){
		hunger.addToHunger (food.GetComponent<Food> ().hungerUp);
		nom.Play ();
		Destroy (food);
	}

	void drop() {
		print ("Dropped a seed");
		holding = false;
		heldObject.transform.parent = GameState.asteroid;
	}

}
