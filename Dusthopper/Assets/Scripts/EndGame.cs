using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

	private GameObject hub;
	private Camera cam;

	private Vector3 camVel;
	private float camScale;

	// Use this for initialization
	void Awake () {
		GameState.endGame = false;
		hub = GameObject.FindGameObjectWithTag ("Hub");
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.O) && !GameState.endGame) {
			End ();
		}

		if (GameState.endGame) {
			cam.transform.position = Vector3.SmoothDamp (cam.transform.position, Vector3.back * 10, ref camVel, 5f);
			cam.orthographicSize = Mathf.SmoothDamp (cam.orthographicSize, 20, ref camScale, 20f);
		}
	}

	public void End () {
		GameObject[] asteroids = GameObject.FindGameObjectsWithTag ("Asteroid");

		foreach (GameObject asteroid in asteroids) {
			asteroid.AddComponent<ConvergeOnHub> ();
		}
		GameState.endGame = true;
		cam.transform.SetParent (null);
		cam.GetComponent<SmoothCamera2D> ().target = null;
		hub.transform.position = Vector3.zero;
		hub.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		hub.GetComponent<Rigidbody2D> ().isKinematic = true;
	}
}
