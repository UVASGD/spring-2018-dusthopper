using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	public string myPollen;

	public void dispenseReward() {
		if (myPollen == "GreenPollen") {
			Debug.Log ("Dispense a reward");
			Destroy (this.gameObject);
		}
	} 
}
