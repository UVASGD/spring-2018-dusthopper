using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManipulator : MonoBehaviour {

	[SerializeField] [Range(0, 10)] private float timeScale = 1;

	private Asteroid[] asteroids;
	private GameObject[] instances;

	private bool mapOpenLF;

	// Use this for initialization
	void Start () {
		mapOpenLF = GameState.mapOpen;
		instances = GameObject.FindGameObjectsWithTag ("Asteroid");
		asteroids = new Asteroid[instances.Length];
		for (int i = 0; i < asteroids.Length; i++) {
			asteroids [i].instance = instances [i].transform;
			//asteroids [i].initialVelocity = instances [i].GetComponent<Rigidbody2D> ().velocity;
			asteroids [i].positions = new Stack<Vector3> (0);
			asteroids [i].velocities = new Stack<Vector3> (0);
		}


		Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI() {
		if (GameState.mapOpen) {
			if (GUI.RepeatButton (new Rect (Screen.width / 2 - 30, Screen.height - 40, 30, 30), "<<")) {
				StartCoroutine ("StepBackward");
			} else {
				//asteroids [i].initialVelocity = asteroids [i].instance.GetComponent<Rigidbody2D> ().velocity;
				Time.timeScale = 0;
			}

			if (GUI.RepeatButton (new Rect (Screen.width / 2 + 30, Screen.height - 40, 30, 30), ">>")) {
				StartCoroutine ("StepForward");
			}

			if (!mapOpenLF) {
				for (int i = 0; i < asteroids.Length; i++) {
					asteroids [i].initialPosition = asteroids [i].instance.position;
					asteroids [i].initialVelocity = asteroids [i].instance.GetComponent<Rigidbody2D>().velocity;
				}
			}
		} else {
			if (mapOpenLF) {
				for (int i = 0; i < asteroids.Length; i++) {
					asteroids [i].instance.position = asteroids [i].initialPosition;
					asteroids [i].instance.GetComponent<Rigidbody2D>().velocity = asteroids [i].initialVelocity;
					asteroids [i].positions.Clear ();
					asteroids [i].velocities.Clear ();
				}
			}
			//print (Time.timeScale);
			Time.timeScale = Mathf.Lerp (Time.timeScale, 1f, 2 * Time.unscaledDeltaTime);
			if (1 - Time.timeScale < 0.1f) {
				Time.timeScale = 1;
			}
		}

		mapOpenLF = GameState.mapOpen;
	}


	IEnumerator StepForward () {
		//print ("Button Pressed!");
		Time.timeScale = timeScale;
		for (int i = 0; i < asteroids.Length; i++) {
			asteroids [i].positions.Push (asteroids [i].instance.position);
			asteroids [i].velocities.Push (asteroids [i].instance.GetComponent<Rigidbody2D>().velocity);

		}
		yield return null;
	}

	IEnumerator StepBackward () {
		for (int i = 0; i < asteroids.Length; i++) {
			if (asteroids [i].positions.Count > 0) {
				asteroids [i].instance.position = asteroids [i].positions.Pop ();
				asteroids [i].instance.GetComponent<Rigidbody2D>().velocity = asteroids [i].velocities.Pop ();
			}
		}
			
		yield return null;
	}

	public struct Asteroid {
		public Transform instance;
		public Vector2 initialVelocity;
		public Vector3 initialPosition;
		public Stack<Vector3> velocities;
		public Stack<Vector3> positions;
	}
}
