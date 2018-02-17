using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hold_Obj : MonoBehaviour {
	bool holding; //Whether or not you are holding something
	GameObject heldObject; //The object being held. The seeds or whatever

	// Use this for initialization
	void Start () {
		holding = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider item) {

	}
}
