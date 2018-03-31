using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFood : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collider)
    {
//        print("Entered Here");
        if (collider.gameObject.name.Equals("Player"))
        {
            GameState.hunger = GameState.maxHunger;
        }
    }
}
