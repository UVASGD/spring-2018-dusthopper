using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

	//This script handles every interaction between the player and an object on the ground

	private Hunger hunger;
	public AudioSource nom;

	void Start(){
		hunger = gameObject.GetComponent<Hunger> ();
	}

	//Add cases for what to do with certain objects here
	void OnTriggerEnter2D ( Collider2D other){
		if (other.gameObject.tag == "Food"){
			print ("EATING FOOD");
			Eat (other.gameObject);
		}
	}

	void Eat(GameObject food){
		hunger.addToHunger (food.GetComponent<Food> ().hungerUp);
		nom.Play ();
		Destroy (food);
	}
}
