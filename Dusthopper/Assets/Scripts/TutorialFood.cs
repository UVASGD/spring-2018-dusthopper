using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFood : MonoBehaviour {
	void Start(){
		Hunger hungus = GameState.player.GetComponent<Hunger> ();
		hungus.addToHunger (-1 * GetComponent<Food> ().hungerUp);
	}
    void OnTriggerEnter2D(Collider2D collider)
    {
//        print("Entered Here");
        if (collider.gameObject.name.Equals("Player"))
        {
            GameState.hunger = GameState.maxHunger;
        }
    }
}
