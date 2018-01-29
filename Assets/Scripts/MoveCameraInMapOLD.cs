using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ryan Kann
// 
// Description
// Allow the player to move the camera around a radius of the current asteroid while the map is open

public class MoveCameraInMapOLD : MonoBehaviour {

	public float camSpeed = 10f;
	private Transform mapCenter; // The center of the map on open (not yet working properly with time effects)

	private Transform camTarg; // What the camera should follow (either the current asteroid or the player)
	private Transform camParent; // Ref to Camera's parent. Camera will always be parented to current asteroid

	private bool mapOpenLF; // Was the map open last frame?

	// Use this for initialization
	void Start () {
		camTarg = GetComponent<SmoothCamera2D> ().target;
		camParent = transform.parent;
		mapCenter = GameObject.Find ("MapCenter").transform;
	}
	
	// Update is called once per frame
	void OnGUI () {
		
		if (GameState.mapOpen) {
			if (!mapOpenLF) {
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
			Vector3 targVel = Vector3.zero;
			if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
				targVel += Vector3.up;
			}
				if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				targVel += Vector3.left;
			}
				if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
				targVel += Vector3.down;
			}
				if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				targVel += Vector3.right;
			}

			targVel = targVel.normalized * camSpeed;
			if ((mapCenter.position + targVel - camTarg.position).sqrMagnitude > GameState.sensorRange * GameState.sensorRange) {
				targVel = Vector3.zero;
			}
//			print (targVel);

			mapCenter.position += (Vector3)targVel * Time.unscaledDeltaTime;
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
