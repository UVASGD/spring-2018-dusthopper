 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrollOut : MonoBehaviour {

	public float scrollSpeed = 5;
	public float minPlayerModeSize; //most the player can zoom in
	public float maxPlayerModeSizeWithMap; //most player can zoom out before entering the map
	public float maxPlayerModeSizeWithoutMap; //most player can zoom out if on an asteroid without map access
	public float minMapModeSize; //Most the player can zoom in in map mode before exiting map mode
	public float mapSize = 20; //zoom amount of map mode (fixed)
	public bool jumpingToAsteroidWithMap = false;


	private float scrollAmount;

	// Use this for initialization
	void Start () {
		scrollAmount = GetComponent<Camera> ().orthographicSize;
	}

	// Update is called once per frame
	void Update () {

		//toggle starfields
		if (GameState.mapOpen) {
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.SetActive (false);
			}
		} else {
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.SetActive (true);
			}
		}

		//if jumping from asteroid without map to asteroid with map, only modify zoom in this way
		if(jumpingToAsteroidWithMap){
			if (GetComponent<Camera> ().orthographicSize < maxPlayerModeSizeWithMap) {
				scrollAmount = GetComponent<Camera> ().orthographicSize;
				jumpingToAsteroidWithMap = false;
			} else {
				scrollAmount = maxPlayerModeSizeWithMap - 0.05f;
				GetComponent<Camera> ().orthographicSize = Mathf.Lerp (GetComponent<Camera> ().orthographicSize, scrollAmount, 10 * Time.unscaledDeltaTime);
				return;
			}
		}

		//normal zooming
		var d = Input.GetAxis ("Mouse ScrollWheel");

		if (d > 0f) {
			scrollAmount += Time.unscaledDeltaTime * scrollSpeed;

			if (GameState.hasSensors) {
				if (scrollAmount > mapSize) {
					scrollAmount = mapSize;
				}
			} else if (scrollAmount > maxPlayerModeSizeWithoutMap) {
				scrollAmount = maxPlayerModeSizeWithoutMap;
			}
		} else if (d < 0f) {
			scrollAmount -= Time.unscaledDeltaTime * scrollSpeed;

			if (scrollAmount < minPlayerModeSize)
				scrollAmount = minPlayerModeSize;
		}
		if (scrollAmount <= maxPlayerModeSizeWithMap) {
			GetComponent<Camera> ().orthographicSize = Mathf.Lerp (GetComponent<Camera> ().orthographicSize, scrollAmount, 10 * Time.unscaledDeltaTime);
		} else {
			if (!GameState.mapOpen) {
				if (GameState.hasSensors) {
					scrollAmount = mapSize;
					GameState.mapOpen = true;
				}
			} else {
				if (scrollAmount < minMapModeSize) {
					scrollAmount = maxPlayerModeSizeWithMap;
					d = 0f;
					GameState.mapOpen = false;
				}
			}
			GetComponent<Camera> ().orthographicSize = Mathf.Lerp (GetComponent<Camera> ().orthographicSize, scrollAmount, 10 * Time.unscaledDeltaTime);
		}
//		print ("orthographic size: " + GetComponent<Camera> ().orthographicSize);
//		print ("scrollAmount: " + scrollAmount);
	}
}
