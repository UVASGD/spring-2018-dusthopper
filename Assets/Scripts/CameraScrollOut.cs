using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finished for now
public class CameraScrollOut : MonoBehaviour {

	public float scrollSpeed; //How sensitive the scroll wheel is ingame.
	public bool swapScroll = false; //A player might choose to invert the scroll wheel from settings.
	public float minPlayerModeSize; //most the player can zoom in
	public float maxPlayerModeSizeWithMap; //most player can zoom out before entering the map
	public float maxPlayerModeSizeWithoutMap; //most player can zoom out if on an asteroid without map access
	public float minMapModeSize; //Most the player can zoom in in map mode before exiting map mode
	private float mapSize = 20f; //zoom amount of map mode (fixed)

	//There is a special zoom procedure for jumping from an asteroid without a map to an asteroid with a map to prevent the map from immediately opening when jump is made.
	//This will be set while that's happening
	public bool jumpingToAsteroidWithMap = false;


	//The target size which the camera will zoom towards
	private float scrollAmount;

	//When entering the map, we want to disable most non-asteroid objects so that they don't do stuff when fast-forwarding in the map.
	//When exiting the map, we want to reenable whatever we disabled.
	//This list caches whatever we disabled last time we entered the map.
	public List<GameObject> disabledObjects;

	// We might need to change this when we have more kinds of asteroids
	public Sprite mapIconWithSensors;
	public Sprite mapIconWithoutSensors;
	public Sprite asteroidSpriteWithSensors;
	public Sprite asteroidSpriteWithoutSensors;

	// Need these until we get map icon art
	public Color iconWithSensor;
	public Color iconWithoutSensor;

	// Use this for initialization
	void Start () {
		scrollAmount = GetComponent<Camera> ().orthographicSize;
		disabledObjects = new List<GameObject> (0);
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
		if (swapScroll) d = d * -1;

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
					SetEnabledNonAsteroids (false);
					GameState.mapOpen = true;
					SwapToMapIcons ();
				}
			} else {
				if (scrollAmount < minMapModeSize) {
					scrollAmount = maxPlayerModeSizeWithMap;
					d = 0f;
					SetEnabledNonAsteroids (true);
					GameState.mapOpen = false;
					SwapToAsteroidSprites();
				}
			}
			GetComponent<Camera> ().orthographicSize = Mathf.Lerp (GetComponent<Camera> ().orthographicSize, scrollAmount, 10 * Time.unscaledDeltaTime);
		}
	}

	//Most objects besides asteroids will get disabled by this when the map is opened and enabled
	public void SetEnabledNonAsteroids (bool enabled) {
		//If you are disabling
		if (!enabled) {
			GameObject[] allObjectsArray = FindObjectsOfType <GameObject> ();

			foreach (GameObject item in allObjectsArray) {
				if (item.layer != LayerMask.NameToLayer("Asteroid") && item.layer != LayerMask.NameToLayer("UI") && item.layer != LayerMask.NameToLayer("Control") && item.layer != LayerMask.NameToLayer("Player")) {
					item.SetActive (false);
					disabledObjects.Add (item);
				}
			}
		} else {
			foreach (GameObject item in disabledObjects) {
				item.SetActive (true);
			}
			disabledObjects.Clear ();
		}
	}

	private void SwapToMapIcons ()
	{
		GameObject[] asteroidList = GameObject.FindGameObjectsWithTag ("Asteroid");
		foreach (GameObject asteroid in asteroidList) {
			asteroid.GetComponent<SpriteRenderer> ().sprite = asteroid.GetComponent<AsteroidInfo> ().mapIcon;
			// We can remove this if/else when we have art
			if (asteroid.GetComponent<AsteroidInfo> ().hasSensors) {
				asteroid.GetComponent<SpriteRenderer> ().color = iconWithSensor;
			} 
			else {
				asteroid.GetComponent<SpriteRenderer> ().color = iconWithoutSensor;
			}
		}
	}

	private void SwapToAsteroidSprites ()
	{
		GameObject[] asteroidList = GameObject.FindGameObjectsWithTag ("Asteroid");
		foreach (GameObject asteroid in asteroidList) {
			asteroid.GetComponent<SpriteRenderer> ().sprite = asteroid.GetComponent<AsteroidInfo>().asteroidSprite;
			// We can remove this if/else when we have art
			if (asteroid.GetComponent<AsteroidInfo> ().hasSensors) {
				asteroid.GetComponent<SpriteRenderer> ().color = asteroid.GetComponent<AsteroidInfo>().hasSensorColor;
			} else {
				asteroid.GetComponent<SpriteRenderer> ().color = Color.white;
			}
		}
	}
}