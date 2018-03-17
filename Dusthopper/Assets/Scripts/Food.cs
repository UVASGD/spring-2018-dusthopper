using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
	public float hungerUp;
    float birthTime;

    private void Start() {
        birthTime = Time.time;
    }

    /*
     * This method is used to limit if this food object can be eaten or not.
     * When food is generated from Pollen objects we want to add a short delay before that food can be eatern.  That is handled by this method.
     */
    public bool canEat() {

        float delayTime = 0.3f; //enough time for player to register that food has been spawned

		if (GameState.time - birthTime >= delayTime) {
            return true;
        } else {
            return false;
        }

    }
}
