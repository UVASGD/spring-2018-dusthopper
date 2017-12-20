 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrollOut : MonoBehaviour {

	public float scrollSpeed = 5;
	public Vector2 sizeRange;
	public float mapSize = 20;

	private float scrollAmount;

	// Use this for initialization
	void Start () {
		scrollAmount = GetComponent<Camera> ().orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {

		if (GameState.mapOpen) {
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.SetActive (false);
			}
		} else {
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.SetActive (true);
			}
		}



		var d = Input.GetAxis ("Mouse ScrollWheel");

		if (d > 0f) {
			scrollAmount += Time.unscaledDeltaTime * scrollSpeed;

			if (scrollAmount > mapSize)
				scrollAmount = mapSize;
		} else if (d < 0f) {
			scrollAmount -= Time.unscaledDeltaTime * scrollSpeed;

			if (scrollAmount < sizeRange.x)
				scrollAmount = sizeRange.x;
		}

		if (scrollAmount <= sizeRange.y) {
			GetComponent<Camera> ().orthographicSize = Mathf.Lerp (GetComponent<Camera> ().orthographicSize, scrollAmount, 10 * Time.unscaledDeltaTime);
		} else {
			if (!GameState.mapOpen) {
				scrollAmount = mapSize;
				GameState.mapOpen = true;
			} else {
				if (scrollAmount < mapSize * 5 / 6) {
					scrollAmount = sizeRange.y;
					d = 0f;
					GameState.mapOpen = false;
				}
			}
			GetComponent<Camera> ().orthographicSize = Mathf.Lerp (GetComponent<Camera> ().orthographicSize, mapSize, 10 * Time.unscaledDeltaTime);
		}
	}
}
