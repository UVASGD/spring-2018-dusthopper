using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour {

//	public Queue<Transform> path;
	public SortedList<float,Transform> path; //key = time to start charging jump , value = transform of target asteroid
	public SortedList<float,float> jumpTimes;
	private GameObject player;
	private bool mapOpenLF;
	private LineRenderer lr;
	private int pathCount = 0;

	private Transform futureAsteroid;

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
		//asteroid = player.GetComponent<Movement> ().asteroid;
		lr = GetComponent<LineRenderer> ();
		GameStateTimeLF = 0f;
		timeSinceChargingStarted = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.mapOpen) {
			EditPath ();
			if (!mapOpenLF) {
				lr.enabled = true;
//				futureAsteroid = GameState.asteroid;
			}
		} else {
			TraversePath ();
			if (mapOpenLF) {
				//path.Clear ();
//				lr.positionCount = 1;
//				pathCount = 0;
				lr.enabled = false;
			}
		}
		lr.SetPosition (0, new Vector3(GameState.asteroid.position.x, GameState.asteroid.position.y, 5f));

		//update path line displays
		for (int i = 1; i < lr.positionCount; i++) {
			lr.SetPosition (i, new Vector3 (path.Values [i - 1].position.x, path.Values [i - 1].position.y, 5f));
		}
		mapOpenLF = GameState.mapOpen;
	}

	void EditPath () {
		//adding to path
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Collider2D hit = Physics2D.OverlapPoint (mousePos);

			if (hit != null && hit.tag == "Asteroid" &&  (hit.transform.position - player.transform.position).sqrMagnitude < (GameState.maxAsteroidDistance * GameState.maxAsteroidDistance)) {
				timeOfJump = GetComponent<TimeManipulator> ().timeFromNow + initialTime;
				timeToStartCharging = timeOfJump - GameState.secondsPerJump;
				//Is it a valid jump?
				if (timeToStartCharging >= initialTime) {
					bool overlap = false;
					int i = 0;
					while (i < path.Keys.Count && !overlap) {
						if (Mathf.Abs (path.Keys [i] - timeToStartCharging) < GameState.secondsPerJump) {
							overlap = true;
							print ("overlaps");
						}
						i++;
					}
					if (!overlap) {
						print ("scheduled jump to asteroid " + hit.transform.gameObject.name + " at time " + timeOfJump);
						path.Add (timeToStartCharging, hit.transform);
						jumpTimes.Add (timeOfJump, timeOfJump);
						lr.positionCount++;
						pathCount++;
						lr.SetPosition (pathCount - 1, new Vector3 (hit.transform.position.x, hit.transform.position.y, 5f));
					}
				} else {
					print ("jump not scheduled because you can't charge in time");
				}
//				print (hit.name + " added to path!");
//				print (path.Count);
			}
		}
		//removing from path
		else if(Input.GetMouseButtonDown(1)){
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Collider2D hit = Physics2D.OverlapPoint (mousePos);

			if (hit != null && hit.tag == "Asteroid"){
				bool foundIt = false;
				int i = 0;
				while (i < path.Keys.Count && !foundIt) {
					if (hit.transform == path.Values[i] && 
					GetComponent<TimeManipulator> ().timeFromNow >= path.Keys [i] && GetComponent<TimeManipulator> ().timeFromNow <= jumpTimes.Keys[i]) {
						foundIt = true;
						print ("removing jump to asteroid " + hit.transform.gameObject.name + " at time " + jumpTimes.Keys [i]);
						path.RemoveAt (i);
						jumpTimes.RemoveAt (i);
						lr.positionCount--;
						pathCount--;
						lr.SetPosition (pathCount - 1, new Vector3 (hit.transform.position.x, hit.transform.position.y, 5f));
					}
					i++;
				}
				if (!foundIt) {
					print ("removed nothing");
				}
			}
		}
	}

	void TraversePath(){
		if (path.Count > 0 && GameState.time >= path.Keys [0]) {
			if (timeSinceChargingStarted >= GameState.secondsPerJump){
				if((path.Values [0].position - player.transform.position).sqrMagnitude < (GameState.maxAsteroidDistance * GameState.maxAsteroidDistance + tolerance)) {
					player.GetComponent<Movement> ().SwitchAsteroid (path.Values [0]);
					path.RemoveAt (0);
					jumpTimes.RemoveAt (0);
				}
				timeSinceChargingStarted = 0f;
			} else if (GameState.time != GameStateTimeLF) {
				timeSinceChargingStarted += Time.deltaTime;
			}
		}
		GameStateTimeLF = GameState.time;
	}
}
