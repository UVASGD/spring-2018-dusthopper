using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour {

	private Hunger hunger;
	private AudioSource asrc;
	public AudioClip nom;

	void Start(){
		hunger = gameObject.GetComponent<Hunger> ();
		asrc = gameObject.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D ( Collider2D other){
		if (other.gameObject.tag == "Food"){
			print ("EATING FOOD");
			Eat (other.gameObject);
		}
	}

	void Eat(GameObject food){
		hunger.addToHunger (food.GetComponent<Food> ().hungerUp);
		asrc.PlayOneShot (nom, 0.5f);
		Destroy (food);
	}
}
