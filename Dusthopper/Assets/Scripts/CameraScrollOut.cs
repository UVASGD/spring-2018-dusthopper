using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finished for now
public class CameraScrollOut : MonoBehaviour {

	public float scrollSpeed; //How sensitive the scroll wheel is ingame.
	[HideInInspector]
	public bool swapScroll = false; //A player might choose to invert the scroll wheel from settings.
	public float minPlayerModeSize; //most the player can zoom in
	public float maxPlayerModeSizeWithMap; //most player can zoom out before entering the map
	public float maxPlayerModeSizeWithoutMap; //most player can zoom out if on an asteroid without map access
	public float minMapModeSize; //Most the player can zoom in in map mode before exiting map mode
	private float mapSize = 20f; //zoom amount of map mode (fixed)

//	public GameObject tooltip;

	//There is a special zoom procedure for jumping from an asteroid without a map to an asteroid with a map to prevent the map from immediately opening when jump is made.
	//This will be set while that's happening
	[HideInInspector]
	public bool jumpingToAsteroidWithMap = false;


	//The target size which the camera will zoom towards
	private float scrollAmount;

	//When entering the map, we want to disable most non-asteroid objects so that they don't do stuff when fast-forwarding in the map.
	//When exiting the map, we want to reenable whatever we disabled.
	//This list caches whatever we disabled last time we entered the map.
	private List<GameObject> disabledObjects;

	//When entering the map, we want to switch asteroid sprites to map icons.
	//When exiting the map, we want switch the sprites back.
	//All the asteroids are children of asteroidContainer
	public GameObject asteroidContainer;
	private GameObject[] theAsteroids;

	// We might need to change this when we have more kinds of asteroids
	public Sprite mapIconWithSensors;
	public Sprite mapIconWithoutSensors;

	// Need these until we get map icon art
	public Color iconWithSensor;
	public Color iconWithoutSensor;

	// Green Plant Icon
	public GameObject greenPlantIcon;

	// Use this for initialization
	void Start () {

		List<GameObject> theAsteroidsTemp = new List<GameObject> ();
		foreach (Transform child in asteroidContainer.transform) {
			theAsteroidsTemp.Add (child.gameObject);
		}
		theAsteroids = theAsteroidsTemp.ToArray ();
		scrollAmount = GetComponent<Camera> ().orthographicSize;
		disabledObjects = new List<GameObject> (0);
	}

	// Update is called once per frame
	void Update () {
		if (GameState.endGame) {
			return;
		}


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

		if (GameState.gamePaused) {//If game paused just don't do any scrolling
			return;
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
		if (swapScroll)
			d = -d;
		
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
					openMap ();
				}
			} else {
				if (scrollAmount < minMapModeSize) {
					scrollAmount = maxPlayerModeSizeWithMap;
					d = 0f;
					SetEnabledNonAsteroids (true);
					GameState.mapOpen = false;
//					tooltip.SetActive (false);
					SwapToAsteroidSprites();
				}
			}
			GetComponent<Camera> ().orthographicSize = Mathf.Lerp (GetComponent<Camera> ().orthographicSize, scrollAmount, 10 * Time.unscaledDeltaTime);
		}

	}

	public void openMap() {
		scrollAmount = mapSize;
		SetEnabledNonAsteroids (false);
		GameState.mapOpen = true;
		//					tooltip.SetActive (true);
		SwapToMapIcons ();
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
				if (item) {
					item.SetActive (true);
				}
			}
			disabledObjects.Clear ();
		}
	}

	private void SwapToMapIcons ()
	{
		foreach (GameObject asteroid in theAsteroids) {
			asteroid.GetComponent<SpriteRenderer> ().sprite = asteroid.GetComponent<AsteroidInfo> ().mapIcon;
			// We can remove this if/else when we have art
			if (asteroid.GetComponent<AsteroidInfo> ().hasSensors) {
				asteroid.GetComponent<SpriteRenderer> ().color = iconWithSensor;
			} 
			else {
				asteroid.GetComponent<SpriteRenderer> ().color = iconWithoutSensor;
			}

			// draw green plant icon
			if (asteroid.GetComponent<AsteroidInfo>().greenPlantCount > 0) {
				GameObject plantIcon = Instantiate(greenPlantIcon, new Vector3 (0,0,0), Quaternion.identity) as GameObject;
				plantIcon.transform.parent = asteroid.gameObject.transform;
				plantIcon.transform.localPosition = new Vector3 (0, 0, 0);
			}
		}
	}

	private void SwapToAsteroidSprites ()
	{
		foreach (GameObject asteroid in theAsteroids) {
			asteroid.GetComponent<SpriteRenderer> ().sprite = asteroid.GetComponent<AsteroidInfo>().asteroidSprite;
			// We can remove this if/else when we have art
			if (asteroid.GetComponent<AsteroidInfo> ().hasSensors) {
				asteroid.GetComponent<SpriteRenderer> ().color = asteroid.GetComponent<AsteroidInfo>().hasSensorColor;
			} else {
				asteroid.GetComponent<SpriteRenderer> ().color = asteroid.GetComponent<AsteroidInfo>().noSensorColor;
			}

			// destroy item icons
			if (asteroid.GetComponent<AsteroidInfo>().greenPlantCount > 0) {
				Transform t = asteroid.transform;
				for (int i = 0; i < t.childCount; i++)
				{
					if(t.GetChild(i).gameObject.tag == "MapIcon")
					{
						Destroy(t.GetChild(i).gameObject);
					}

				}
			}

		}
	}

	public void SwapInvertScroll () {
		swapScroll = !swapScroll;
	}

	public void SetScrollSpeed (float val) {
		scrollSpeed = val;
	}
}