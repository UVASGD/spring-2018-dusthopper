using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour {

//	public Queue<Transform> path;
//	lr.material = new Material(Shader.Find("Particles/Additive (Soft)"));
	public SortedList<float,Transform> path; //key = time to start charging jump , value = transform of target asteroid
	public SortedList<float,float> jumpTimes;
	private GameObject player;
	private bool mapOpenLF;
	private List<GameObject> lines;
	public float percentIdle;
	public float timeTakesToJump;
//	private LineRenderer lr;
//	private int pathCount = 0;

//	private Transform futureAsteroid;

	public float initialTime;
	private float timeOfJump;
	private float timeToStartCharging;
	private float timeSinceChargingStarted;
	private float GameStateTimeLF;
	public float tolerance; //When final jump is made, I want to be a bit lenient with max distance because of various ways the jump may have changed since being scheduled.

	// Use this for initialization
	void Start () {
		path = new SortedList<float,Transform> (0);
		jumpTimes = new SortedList<float, float> (0);
		player = GameObject.FindWithTag ("Player");
		lines = new List<GameObject> ();
		GameStateTimeLF = 0f;
		timeSinceChargingStarted = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.mapOpen) {
			EditPath ();
			if (!mapOpenLF) {
				foreach (GameObject line in lines) {
					line.GetComponent<LineRenderer> ().enabled = true;
				}
			}
		} else {
			TraversePath ();
			if (mapOpenLF) {
				foreach (GameObject line in lines) {
					line.GetComponent<LineRenderer> ().enabled = false;
				}
			}
		}
		if (lines.Count > 0) {
			lines[0].GetComponent<LineRenderer> ().SetPosition (0, new Vector3 (GameState.asteroid.position.x, GameState.asteroid.position.y, 5f));
			lines[0].GetComponent<LineRenderer> ().SetPosition (1, new Vector3 (path.Values[0].position.x, path.Values[0].position.y, 5f));
			float a = Alpha (GetComponent<TimeManipulator> ().timeFromNow + initialTime, 0);
			lines[0].GetComponent<LineRenderer> ().startColor = new Color(1f,0.69f,0f,a);
			lines[0].GetComponent<LineRenderer> ().endColor = new Color(1f,0.69f,0f,a);
			for(int i = 1; i < path.Count; i++){
				lines[i].GetComponent<LineRenderer> ().SetPosition (0, new Vector3 (path.Values[i-1].position.x, path.Values[i-1].position.y, 5f));
				lines[i].GetComponent<LineRenderer> ().SetPosition (1, new Vector3 (path.Values[i].position.x, path.Values[i].position.y, 5f));
				a = Alpha (GetComponent<TimeManipulator> ().timeFromNow + initialTime, i);
				lines[i].GetComponent<LineRenderer> ().startColor = new Color(1f,0.69f,0f,a);
				lines[i].GetComponent<LineRenderer> ().endColor = new Color(1f,0.69f,0f,a);
			}
			
		}
//		foreach (GameObject line in lines) {
//			print("line
//		}
		mapOpenLF = GameState.mapOpen;
	}

	float Alpha(float time, int pathIndex){
		float chargeTime = path.Keys [pathIndex];
		float jumpTime = jumpTimes.Keys [pathIndex];
		if (time >= chargeTime && time <= jumpTime) {
			return ((1 - percentIdle) / GameState.secondsPerJump) * (time - chargeTime) + percentIdle;
		}
		if(time >= jumpTime){
			if (time <= jumpTime + timeTakesToJump) {
				return 1;
			}
			if (time <= jumpTime + GameState.secondsPerJump) {
				return  1 - (((1 - percentIdle) / GameState.secondsPerJump) * (time - jumpTime) + percentIdle);
			}
		}
		return percentIdle;
	}
	void EditPath () {
		//click to toggle adding / removing from path
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Collider2D hit = Physics2D.OverlapPoint (mousePos);

			if (hit != null && hit.tag == "Asteroid" && hit != GameState.asteroid) {
				//Try to find the jump in the current path
				bool foundIt = false;
				int i = 0;
				while (i < path.Keys.Count && !foundIt) {
					if (hit.transform == path.Values[i] && 
						GetComponent<TimeManipulator> ().timeFromNow >= path.Keys [i] && GetComponent<TimeManipulator> ().timeFromNow <= jumpTimes.Keys[i]) {
						foundIt = true;
						print ("removing jump to asteroid " + hit.transform.gameObject.name + " at time " + jumpTimes.Keys [i]);
						path.RemoveAt (i);
						jumpTimes.RemoveAt (i);
						lines.RemoveAt (i);
					}
					i++;
				}
				if (!foundIt) {//If you didn't find a jump to remove, the player is trying to add a jump
					timeOfJump = GetComponent<TimeManipulator> ().timeFromNow + initialTime;
					timeToStartCharging = timeOfJump - GameState.secondsPerJump;
					//Is it a valid jump?
					if (timeToStartCharging >= initialTime) {
						bool overlap = false;
						i = 0;
						while (i < path.Keys.Count && !overlap) {
							if (Mathf.Abs (path.Keys [i] - timeToStartCharging) < GameState.secondsPerJump) {
								overlap = true;
								print ("jump not scheduled because it overlaps with an existing jump");
							}
							i++;
						}
						if (!overlap) {
							print ("scheduled jump to asteroid " + hit.transform.gameObject.name + " at time " + timeOfJump);
							path.Add (timeToStartCharging, hit.transform);
							jumpTimes.Add (timeOfJump, timeOfJump);
							GameObject newLine = new GameObject ();
							newLine.name = "Line" + i.ToString ();
							newLine.transform.parent = transform;
							newLine.AddComponent (typeof(LineRenderer));
							newLine.GetComponent<LineRenderer> ().material = new Material(Shader.Find ("Sprites/Default"));
							newLine.GetComponent<LineRenderer> ().startColor = new Color (1f,0.69f,0f,1);
							newLine.GetComponent<LineRenderer> ().endColor = new Color (1f,0.69f,0f,1);
							newLine.GetComponent<LineRenderer> ().startWidth = 0.3f;
							newLine.GetComponent<LineRenderer> ().endWidth = 0.3f;
							newLine.GetComponent<LineRenderer> ().positionCount = 2;
							lines.Add (newLine);
						}
					} else {
						print ("jump not scheduled because you can't charge in time");
					}
				}
			}
		}
	}

	void TraversePath(){
		if (path.Count > 0 && GameState.time >= path.Keys [0]) {
			if (timeSinceChargingStarted >= GameState.secondsPerJump){
				if ((path.Values [0].position - player.transform.position).sqrMagnitude < (GameState.maxAsteroidDistance * GameState.maxAsteroidDistance + tolerance)) {
					print ("jumping to asteroid " + path.Values [0] + " at time " + GameState.time);
					player.GetComponent<Movement> ().SwitchAsteroid (path.Values [0]);
				} else {
					print ("jump cancelled - too far");
				}
				path.RemoveAt (0);
				jumpTimes.RemoveAt (0);
				lines.RemoveAt (0);
				timeSinceChargingStarted = 0f;
			} else if (GameState.time != GameStateTimeLF) {
				timeSinceChargingStarted += Time.deltaTime;
			}
		}
		GameStateTimeLF = GameState.time;
	}
}
