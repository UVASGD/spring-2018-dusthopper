using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManipulator : MonoBehaviour {
	//This script handles the fast forward / rewind stuff in the map, including the GUI
	//TODO: make the GUI more attractive

	[SerializeField] [Range(0, 10)] private float timeScale = 10f;

	private Asteroid[] asteroids;
	private List<GameObject> instances;
	private Stack<float> frameTimes;
	public GameObject asteroidContainer;
	[HideInInspector]
	public float timeFromNow;
	public float reallySmall; //a really small number which eliminates floating point equality bullshit after autoscrolling to exactly secondsPerJump seconds into the future.

	//Text field for how many seconds in future we are
	private float timeToDisplay; //will be timeFromNow unless we just stopped autoscrolling in which case it's timeFromNow - reallySmall (which will be a nice round number)
	private GUIStyle style;
	private Rect rect;

	//Text fields for how many seconds in future we are allowed to go, and 0.00 (just for aesthetic)
	private Rect rect2;
	private Rect rect3;

	public bool autoScroll;
	private bool autoScrollLF;
	private float startScrollTime;

	private bool mapOpenLF;
	private bool timeMoving = false;

	// Use this for initialization
	void Start () {
		timeMoving = false;
		autoScroll = false;
		autoScrollLF = false;
		instances = new List<GameObject> ();
		//AutoScroll ();
		mapOpenLF = GameState.mapOpen;
		foreach (Transform child in asteroidContainer.transform) {
			instances.Add (child.gameObject);
		}
		asteroids = new Asteroid[instances.Count];
		for (int i = 0; i < asteroids.Length; i++) {
			asteroids [i].instance = instances [i].transform;
			//asteroids [i].initialVelocity = instances [i].GetComponent<Rigidbody2D> ().velocity;
			asteroids [i].positions = new Stack<Vector3> (0);
			asteroids [i].velocities = new Stack<Vector3> (0);
//			asteroids [i].rotations = new Stack<Quaternion> (0);
//			asteroids [i].angularVelocities = new Stack<float> (0);
		}
		frameTimes = new Stack<float> (0);
		Time.timeScale = 0;

		//Time text field
		style = new GUIStyle();
		rect = new Rect(Screen.width / 2, Screen.height - 70, 30, 30);
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = new Color (1f, 1f, 1f, 1.0f);

		rect2 = new Rect(Screen.width / 2 + 80, Screen.height - 70, 30, 30);

		rect3 = new Rect(Screen.width / 2 - 80, Screen.height - 70, 30, 30);
	}
	
//	// Update is called once per frame
	void Update () {
		if (autoScroll) {
			StartCoroutine ("StepForward");
		}		
	}

	public void AutoScroll () {
//		print ("Scrolling!");
		startScrollTime = timeFromNow;
		autoScroll = true;
	}

	void OnGUI() {
		if (GameState.mapOpen) {
//			print ("Time elapsed: " + (Time.time - startScrollTime));
			if (timeFromNow - startScrollTime >= GameState.secondsPerJump) {
				if (autoScroll) {
//					print ("stopped autoscroll");
					timeFromNow = startScrollTime + GameState.secondsPerJump + reallySmall;
					timeToDisplay = startScrollTime + GameState.secondsPerJump;
				}
				autoScroll = false;
			}

			if (GUI.RepeatButton (new Rect (Screen.width / 2 - 40, Screen.height - 70, 30, 30), "<<")) {
				autoScroll = false;
				StartCoroutine ("StepBackward");
			} else {
				//asteroids [i].initialVelocity = asteroids [i].instance.GetComponent<Rigidbody2D> ().velocity;
				if (!autoScroll) {
					Time.timeScale = 0;
				}
			}

			if (GUI.RepeatButton (new Rect (Screen.width / 2 + 40, Screen.height - 70, 30, 30), ">>")) {
				if (timeFromNow < GameState.sensorTimeRange) {
					autoScroll = false;
					StartCoroutine ("StepForward");
				}
			}

			if (timeFromNow > GameState.sensorTimeRange) {
				autoScroll = false;
				timeFromNow = GameState.sensorTimeRange;
				timeToDisplay = timeFromNow;
			}
			string zero = "0.00";
			GUI.Label (rect3, zero, style);

			string text = string.Format("{0:0.00}",timeToDisplay);
			GUI.Label(rect, text, style);

			string text2 = string.Format("{0:0.00}",GameState.sensorTimeRange);
			GUI.Label(rect2, text2, style);

			if (!mapOpenLF) {
				timeFromNow = 0f;
				AutoScroll ();

				for (int i = 0; i < asteroids.Length; i++) {
					asteroids [i].initialPosition = asteroids [i].instance.position;
					asteroids [i].initialVelocity = asteroids [i].instance.GetComponent<Rigidbody2D>().velocity;
//					asteroids [i].initialRotation = asteroids [i].instance.rotation;
//					asteroids [i].initialAngularVelocity = asteroids [i].instance.GetComponent<Rigidbody2D> ().angularVelocity;
				}
			}
		} else {
			if (mapOpenLF) {
				for (int i = 0; i < asteroids.Length; i++) {
					asteroids [i].instance.position = asteroids [i].initialPosition;
					asteroids [i].instance.GetComponent<Rigidbody2D>().velocity = asteroids [i].initialVelocity;
//					asteroids [i].instance.rotation = asteroids [i].initialRotation;
//					asteroids [i].instance.GetComponent<Rigidbody2D> ().angularVelocity = asteroids [i].initialAngularVelocity;
					asteroids [i].positions.Clear ();
					asteroids [i].velocities.Clear ();
//					asteroids [i].rotations.Clear ();
//					asteroids [i].angularVelocities.Clear ();
				}
				frameTimes.Clear ();
			}
			//print (Time.timeScale);
			if (!GameState.gamePaused) {
				Time.timeScale = Mathf.Lerp (Time.timeScale, 1f, 2 * Time.unscaledDeltaTime);
				if (1 - Time.timeScale < 0.1f) {
					Time.timeScale = 1;
				}
			}
		}

		mapOpenLF = GameState.mapOpen;
		autoScrollLF = autoScroll;
	}


	IEnumerator StepForward () {
//		print ("calling stepForward - time currently is " + timeFromNow);
		timeMoving = true;
		//print ("Button Pressed!");
		Time.timeScale = timeScale;
		for (int i = 0; i < asteroids.Length; i++) {
			asteroids [i].positions.Push (asteroids [i].instance.position);
			asteroids [i].velocities.Push (asteroids [i].instance.GetComponent<Rigidbody2D>().velocity);
//			asteroids [i].rotations.Push (asteroids [i].instance.rotation);
//			asteroids [i].angularVelocities.Push (asteroids [i].instance.GetComponent<Rigidbody2D> ().angularVelocity);

		}
		timeFromNow += Time.deltaTime;
		timeToDisplay = timeFromNow;
		frameTimes.Push (timeFromNow);
		timeMoving = false;
		yield return null;
	}

	IEnumerator StepBackward () {
		timeMoving = true;
		if (frameTimes.Count != 0) {
			for (int i = 0; i < asteroids.Length; i++) {
				if (asteroids [i].positions.Count > 0) {
					asteroids [i].instance.position = asteroids [i].positions.Pop ();
					asteroids [i].instance.GetComponent<Rigidbody2D> ().velocity = asteroids [i].velocities.Pop ();
//					asteroids [i].instance.rotation = asteroids [i].rotations.Pop ();
//					asteroids [i].instance.GetComponent<Rigidbody2D> ().angularVelocity = asteroids [i].angularVelocities.Pop ();
				}
			}
			timeFromNow = frameTimes.Pop ();
			timeToDisplay = timeFromNow;
		}
		timeMoving = false;
		yield return null;
	}

	public struct Asteroid {
		public Transform instance;
		public Vector2 initialVelocity;
		public Vector3 initialPosition;
//		public Quaternion initialRotation;
//		public float initialAngularVelocity;
		public Stack<Vector3> velocities;
		public Stack<Vector3> positions;
//		public Stack<Quaternion> rotations;
//		public Stack<float> angularVelocities;
	}
}
