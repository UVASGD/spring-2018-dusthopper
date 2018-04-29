using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finished for now
public class MoveCameraInMap : MonoBehaviour {
	//The camera movement in the map is based on a different system because time is not moving unless fast-forwarding is being done, so we can't use Update()
	//We instead use OnGUI() and Time.unscaledDeltaTime

	public float camSpeed = 10f;
	private Transform mapCenter;

	private Transform camTarg;
	private Transform camParent;

	private bool mapOpenLF;

	private float startJumpDistance;
	private float currentJumpDistance;
	private float jumpDifference;

	// Use this for initialization
	void Start () {
		camTarg = GetComponent<SmoothCamera2D> ().target;
		camParent = transform.parent;
		mapCenter = GameObject.Find ("MapCenter").transform;
		startJumpDistance = GameState.defaultMaxAsteroidDistance;
		currentJumpDistance = GameState.maxAsteroidDistance;
		jumpDifference = currentJumpDistance/startJumpDistance * 2 / 3;
	}
	
	// Update is called once per frame
	void OnGUI () {
//		print (jumpDifference);
		if (GameState.mapOpen) {
			if (!mapOpenLF) {
				startJumpDistance = GameState.defaultMaxAsteroidDistance;
				currentJumpDistance = GameState.maxAsteroidDistance;
				jumpDifference = currentJumpDistance/startJumpDistance * 2 / 3;

				camTarg = GetComponent<SmoothCamera2D> ().target;
				GetComponent<SmoothCamera2D> ().target = mapCenter;
				camParent = transform.parent;
				transform.parent = null;
			}

			/*Vector2 targVel = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized * camSpeed;

			if ((Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0)) {
				targVel = Vector3.zero;
			}
			print (targVel);*/
			if (!GameState.gamePaused) {
				Vector3 targVel = Vector3.zero;
				if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
					targVel += Vector3.up;
				}
				if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
					targVel += Vector3.left;
				}
				if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
					targVel += Vector3.down;
				}
				if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
					targVel += Vector3.right;
				}

				targVel = targVel.normalized * camSpeed;
				if ((mapCenter.position + targVel - camTarg.position).magnitude > GameState.sensorRange * jumpDifference) {
					//shunt back toward player
					mapCenter.position += (Vector3)(camParent.position - mapCenter.position).normalized * camSpeed * Time.unscaledDeltaTime;
				} else {
					mapCenter.position += (Vector3)targVel * Time.unscaledDeltaTime;
				}
			}
//			print (targVel);

		} else {
			if (mapOpenLF) {
				GetComponent<SmoothCamera2D> ().target = camTarg;
				transform.parent = camParent;
			}
			mapCenter.position = GameState.asteroid.position;
		}

		mapOpenLF = GameState.mapOpen;
	}
}
