﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManipulator : MonoBehaviour {
	//This script handles the fast forward / rewind stuff in the map, including the GUI
	//TODO: make the GUI more attractive

	[SerializeField] [Range(0, 10)] private float timeScale = 10f;

	private Asteroid[] asteroids;
    private WindMakerStruct[] windMakers;
	private List<GameObject> instances;
    private List<GameObject> windInstances;
    private WindSimulationStruct windSimulation;
	private Stack<float> frameTimes;
	public GameObject asteroidContainer;
    public GameObject windContainer;
	[HideInInspector]
	public float timeFromNow;

	//Text field for how many seconds in future we are
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
        windInstances = new List<GameObject>();
		//AutoScroll ();
		mapOpenLF = GameState.mapOpen;
		foreach (Transform child in asteroidContainer.transform) {
			instances.Add (child.gameObject);
		}
        foreach (Transform child in windContainer.transform){
            windInstances.Add(child.gameObject);   
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
        windMakers = new WindMakerStruct[windInstances.Count];
        for (int i = 0; i < windMakers.Length; i++)
        {
            windMakers[i].positions = new Stack<Vector3>(0);
            windMakers[i].rotations = new Stack<Quaternion>(0);
            windMakers[i].windDirection = new Stack<Vector3>(0);
        }
        windSimulation = new WindSimulationStruct();
        windSimulation.randomStates = new Stack<Random.State>(0);
        windSimulation.simStep = new Stack<int>(0);
        windSimulation.windExist = new Stack<bool>(0);

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
					timeFromNow = startScrollTime + GameState.secondsPerJump + 0.0001f;
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
			}
			string zero = "0.00";
			GUI.Label (rect3, zero, style);

			string text = string.Format("{0:0.00}",Mathf.Max(timeFromNow, 0f));
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
                for (int i = 0; i < windMakers.Length; i++)
                {
                    windMakers[i].initialPosition = windInstances[i].transform.position;
                    windMakers[i].initialRotation = windInstances[i].transform.rotation;
                    windMakers[i].initialWindDirection = windInstances[i].GetComponent<WindMaker>().windDirection;
                }
                windSimulation.initialSimStep = GameState.currentWindSimStep;
                windSimulation.initialRandomState = Random.state;
                windSimulation.initialWindExist = GameState.windExist;
			}
		} else {
			if (mapOpenLF) {
				autoScroll = false;
				StopCoroutine ("StepForward");
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
                for (int i = 0; i < windMakers.Length; i++)
                {
                    windInstances[i].transform.position = windMakers[i].initialPosition;
                    windInstances[i].transform.rotation = windMakers[i].initialRotation;
                    windInstances[i].GetComponent<WindMaker>().windDirection = windMakers[i].initialWindDirection;
                    windInstances[i].SetActive(windSimulation.initialWindExist);
                    windMakers[i].positions.Clear();
                    windMakers[i].windDirection.Clear();
                }
                GameState.currentWindSimStep = windSimulation.initialSimStep;
                Random.state = windSimulation.initialRandomState;
                GameState.windExist = windSimulation.initialWindExist;
                windSimulation.randomStates.Clear();
                windSimulation.simStep.Clear();
                windSimulation.windExist.Clear();
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
		timeMoving = true;
		//print ("Button Pressed!");
		Time.timeScale = timeScale;
		for (int i = 0; i < asteroids.Length; i++) {
			asteroids [i].positions.Push (asteroids [i].instance.position);
			asteroids [i].velocities.Push (asteroids [i].instance.GetComponent<Rigidbody2D>().velocity);
//			asteroids [i].rotations.Push (asteroids [i].instance.rotation);
//			asteroids [i].angularVelocities.Push (asteroids [i].instance.GetComponent<Rigidbody2D> ().angularVelocity);
		}
        for (int i = 0; i < windMakers.Length; i++)
        {
            windMakers[i].positions.Push(windInstances[i].transform.position);
            windMakers[i].rotations.Push(windInstances[i].transform.rotation);
            windMakers[i].windDirection.Push(windInstances[i].GetComponent<WindMaker>().windDirection);
        }
        windSimulation.randomStates.Push(Random.state);
        windSimulation.simStep.Push(GameState.currentWindSimStep);
        windSimulation.windExist.Push(GameState.windExist);
		timeFromNow += Time.deltaTime;
		frameTimes.Push (timeFromNow);
        GameState.time = GameState.lastGameTime + timeFromNow;
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
            for (int i = 0; i < windMakers.Length; i++)
            {
                if (windMakers[i].positions.Count > 0)
                {
                    windInstances[i].transform.position = windMakers[i].positions.Pop();
                    windInstances[i].transform.rotation = windMakers[i].rotations.Pop();
                    windInstances[i].GetComponent<WindMaker>().windDirection = windMakers[i].windDirection.Pop();
                    windInstances[i].SetActive(windSimulation.windExist.Peek());
                }
            }
            Random.state = windSimulation.randomStates.Pop();
            GameState.currentWindSimStep = windSimulation.simStep.Pop();
            GameState.windExist = windSimulation.windExist.Pop();
            timeFromNow = frameTimes.Pop ();
            GameState.time = GameState.lastGameTime + timeFromNow;
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

    public struct WindMakerStruct
    {
        public Vector3 initialPosition;
        public Quaternion initialRotation;
        public Vector3 initialWindDirection;
        public Stack<Vector3> positions;
        public Stack<Quaternion> rotations;
        public Stack<Vector3> windDirection;
    }

    public struct WindSimulationStruct
    {
        public int initialSimStep;
        public Random.State initialRandomState;
        public bool initialWindExist;
        public Stack<Random.State> randomStates;
        public Stack<int> simStep;
        public Stack<bool> windExist;
    }
}
