using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour {

	public Queue<Transform> path;
	private GameObject player;
	private bool mapOpenLF;
	private LineRenderer lr;
	private int pathCount = 0;
	//private Transform asteroid;

	// Use this for initialization
	void Start () {
		path = new Queue<Transform> (0);
		player = GameObject.FindWithTag ("Player");
		//asteroid = player.GetComponent<Movement> ().asteroid;
		lr = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.mapOpen) {
			AddToPath ();
		} else if (mapOpenLF) {
			//path.Clear ();
			lr.positionCount = 1;
			pathCount = 0;

			StartCoroutine ("TraversePath");
		}
		lr.SetPosition (0, new Vector3(GameState.asteroid.position.x, GameState.asteroid.position.y, 5f));

		Transform[] arrayPath = path.ToArray ();
		for (int i = 1; i < lr.positionCount; i++) {
			lr.SetPosition (i, new Vector3(arrayPath[i-1].position.x, arrayPath[i-1].position.y, 5f));
		}

		mapOpenLF = GameState.mapOpen;
	}

	void AddToPath () {
		//print ("Add to path called");
		if (Input.GetMouseButtonDown(0))
		{
		//	print ("Button press");
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Collider2D hit = Physics2D.OverlapPoint (mousePos);

			if (hit != null && hit.tag == "Asteroid") {
				path.Enqueue (hit.transform);
				lr.positionCount++;
				pathCount++;
				lr.SetPosition (pathCount - 1, new Vector3 (hit.transform.position.x, hit.transform.position.y, 5f));
//				print (hit.name + " added to path!");
//				print (path.Count);
			}
		}
	}

	IEnumerator TraversePath () {
		yield return new WaitForSeconds (GameState.secondsPerJump);
		int stepCount = path.Count;
		for (int i = 0; i < stepCount; i++) {
			//print ("Step " + (i+1) + " out of " + path.Count);
			if ((path.Peek ().position - player.transform.position).sqrMagnitude < (GameState.maxAsteroidDistance * GameState.maxAsteroidDistance)) {
				if (path.Count > 0) {
					//print ("Moving to " + path.Peek().name);
					player.GetComponent<Movement> ().SwitchAsteroid (path.Dequeue ());

					yield return new WaitForSeconds (GameState.secondsPerJump);
				} else {
					print ("End of path!");
				}
			} else {
				path.Clear ();
				print ("Next asteroid out of range, ending path.");
				break;
			}
		}

	}
}
