using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

	private bool theEnd;
	private GameObject hub;

	// Use this for initialization
	void Awake () {
		theEnd = false;
		hub = GameObject.FindGameObjectWithTag ("Hub");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.O)) {
			End ();
		}
	}

	public void End () {
		GameObject[] asteroids = GameObject.FindGameObjectsWithTag ("Asteroid");

		foreach (GameObject asteroid in asteroids) {
			asteroid.AddComponent<ConvergeOnHub> ();
		}
	}
}
