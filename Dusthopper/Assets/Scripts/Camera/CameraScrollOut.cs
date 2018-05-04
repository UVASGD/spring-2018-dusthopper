using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finished for now
public class CameraScrollOut : MonoBehaviour {

    public Sprite PLAYERICO;
    public float scaleFactor = 7;

	public float scrollSpeed; //How sensitive the scroll wheel is ingame.
	[HideInInspector]
	public bool swapScroll = false; //A player might choose to invert the scroll wheel from settings.
	public float minPlayerModeSize; //most the player can zoom in
	public float maxPlayerModeSizeWithMap; //most player can zoom out before entering the map
	public float maxPlayerModeSizeWithoutMap; //most player can zoom out if on an asteroid without map access
	public float minMapModeSize; //Most the player can zoom in in map mode before exiting map mode
	private float mapSize = 20f; //zoom amount of map mode (fixed)
	private const float plantIconOffset = 0.75f;

//	public GameObject tooltip;

	//There is a special zoom procedure for jumping from an asteroid without a map to an asteroid with a map to prevent the map from immediately opening when jump is made.
	//This will be set while that's happening
	[HideInInspector]
	public bool jumpingToAsteroidWithMap = false;
	public bool justGotGrayPollen = false;


	//The target size which the camera will zoom towards
	private float scrollAmount;

	private float d;

	//When entering the map, we want to disable most non-asteroid objects so that they don't do stuff when fast-forwarding in the map.
	//When exiting the map, we want to reenable whatever we disabled.
	//This list caches whatever we disabled last time we entered the map.
	private List<GameObject> disabledObjects;

	//When entering the map, we want to switch asteroid and ScapCloudCore sprites to map icons.
	//When exiting the map, we want switch the sprites back.
	//All the asteroids are children of asteroidContainer, and ALL ScrapClouds are children of scrapCloudContainer
	public GameObject asteroidContainer;
	private GameObject[] theAsteroids;
    public GameObject scrapCloudsContainer;
    private GameObject[] theScrapClouds;

    // We might need to change this when we have more kinds of asteroids
    public Sprite mapIconWithSensors;
	public Sprite mapIconWithoutSensors;

	// Plant Icons
	public GameObject greenPlantIcon;
	public GameObject bluePlantIcon;
	public GameObject yellowPlantIcon;
	public GameObject redPlantIcon;
	private int numIcons;

	// Use this for initialization
	void Start () {

		List<GameObject> theAsteroidsTemp = new List<GameObject> ();
		foreach (Transform child in asteroidContainer.transform) {
			theAsteroidsTemp.Add (child.gameObject);
		}
        List<GameObject> theScrapCloudsTemp = new List<GameObject>();
        foreach (Transform child in scrapCloudsContainer.transform) {
            theScrapCloudsTemp.Add (child.gameObject);
        }
		theAsteroids = theAsteroidsTemp.ToArray ();
        theScrapClouds = theScrapCloudsTemp.ToArray ();
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
			//Turn off Starfields
			for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.SetActive (false);
			}
		} else {
			//Turn on Starfields
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
		d = Input.GetAxis ("Mouse ScrollWheel");
        GameObject jumpIndicator = GameObject.Find("JumpIndicator");
        if (jumpIndicator != null) {
            jumpIndicator.transform.localScale = (scaleFactor + .5f*scrollAmount) * Vector3.one;
        }


        if (Input.GetButton("Zoom In")) d = scrollSpeed;
        if (Input.GetButton("Zoom Out")) d = -scrollSpeed;

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
					closeMap ();
				}
			}
			GetComponent<Camera> ().orthographicSize = Mathf.Lerp (GetComponent<Camera> ().orthographicSize, scrollAmount, 10 * Time.unscaledDeltaTime);
		}

	}

	public void openMap() {
		scrollAmount = mapSize;
		SetEnabledNonAsteroids (false);
		GameState.mapOpen = true;
        GameState.lastGameTime = GameState.time;
        //					tooltip.SetActive (true);
        SwapToMapIcons ();
	}

	public void closeMap(){
		scrollAmount = maxPlayerModeSizeWithMap;
		d = 0f;
		SetEnabledNonAsteroids (true);
		GameState.mapOpen = false;
		GameState.time = GameState.lastGameTime;
		//					tooltip.SetActive (false);
		if (justGotGrayPollen) {
			justGotGrayPollen = false;
			GameState.maxAsteroidDistance = GameState.maxAsteroidDistance / GameState.grayDistFactor;
		}
		SwapToAsteroidSprites();
	}

	//Most objects besides asteroids will get disabled by this when the map is opened and enabled
	public void SetEnabledNonAsteroids (bool enabled) {
		//If you are disabling
		if (!enabled) {
			GameObject[] allObjectsArray = FindObjectsOfType <GameObject> ();

			foreach (GameObject item in allObjectsArray) {
                if (item.name.ToLower().Contains("selectionindicator"))
                    Destroy(item);
				else if (item.layer != LayerMask.NameToLayer("Asteroid") && item.layer != 
                    LayerMask.NameToLayer("UI") && item.layer != 
                    LayerMask.NameToLayer("Control") && item.layer != 
                    LayerMask.NameToLayer("Player")) {
					
					item.SetActive (false);
					disabledObjects.Add (item);
				}
			}
		} else {
			foreach (GameObject item in disabledObjects) {
				if (item) {
					item.SetActive (true);

                    //if this item is a ScrapInCloud prefab assign it a new, random velocity
                    if (item.layer == LayerMask.NameToLayer("SpaceScrap")) {
                        float maxSpeed = 1f;
                        item.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * maxSpeed;
                    }
                }
            }
			disabledObjects.Clear ();
		}
	}

	private void SwapToMapIcons () {
        foreach (GameObject asteroid in theAsteroids) {
            AsteroidInfo aI = asteroid.GetComponent<AsteroidInfo>();
            SpriteRenderer sR = asteroid.GetComponent<SpriteRenderer>();
            asteroid.GetComponent<SpriteRenderer>().sprite = aI.SensorMapIcon;
            sR.color = Color.white;
            if (GameState.asteroid.gameObject == asteroid.gameObject) {
                sR.sprite = PLAYERICO;
                //Debug.Log("MEMES ARE A HEALTHY DOSAGE OF MEDICINE REQUIRED TO DO THE THING YOU NEED TO LIVE YOUR LIVER");
            } else {
                if (aI.hasSensors) {
                    sR.sprite = aI.SensorMapIcon;
                } else {
                    sR.sprite = aI.NonSenseMapIcon;
                }
            }

            // highlight them if they are in current path
            PathMaker pm = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PathMaker> ();
			//if (pm.path.ContainsValue (asteroid.transform)) {
			//	asteroid.GetComponent<SpriteRenderer> ().color = new Color (sR.color.r + pm.highlightAmount, 
   //                 sR.color.g + pm.highlightAmount, sR.color.b + pm.highlightAmount, 1);
			//}

			// draw plant icons
			if (aI.greenPlantCount > 0) {
				GameObject plantIcon = Instantiate(greenPlantIcon, new Vector3 (0,0,0), Quaternion.identity) as GameObject;
				plantIcon.transform.parent = asteroid.gameObject.transform;
				plantIcon.transform.localPosition = new Vector3 (0,plantIconOffset, 0);
			}
			if (aI.bluePlantCount > 0) {
				GameObject plantIcon = Instantiate(bluePlantIcon, new Vector3 (0,0,0), Quaternion.identity) as GameObject;
				plantIcon.transform.parent = asteroid.gameObject.transform;
				plantIcon.transform.localPosition = new Vector3 (0, -plantIconOffset, 0);
			}
			if (aI.yellowPlantCount > 0) {
				GameObject plantIcon = Instantiate(yellowPlantIcon, new Vector3 (0,0,0), Quaternion.identity) as GameObject;
				plantIcon.transform.parent = asteroid.gameObject.transform;
				plantIcon.transform.localPosition = new Vector3 (plantIconOffset, 0, 0);
			}
			if (aI.redPlantCount > 0) {
				GameObject plantIcon = Instantiate(redPlantIcon, new Vector3 (0,0,0), Quaternion.identity) as GameObject;
				plantIcon.transform.parent = asteroid.gameObject.transform;
				plantIcon.transform.localPosition = new Vector3 (-plantIconOffset, 0, 0);
			}
		}

        //also swap to mapIcons for ScapCloudCores
        foreach (GameObject cloud in theScrapClouds) {
            cloud.GetComponent<SpriteRenderer>().sprite = cloud.GetComponent<AsteroidInfo>().SensorMapIcon;
        }

	}

	private void SwapToAsteroidSprites ()
	{
		foreach (GameObject asteroid in theAsteroids) {
			asteroid.GetComponent<SpriteRenderer> ().sprite = asteroid.GetComponent<AsteroidInfo>().asteroidSprite;
            // We can remove this if/else when we have art
            if (asteroid.GetComponent<AsteroidInfo>().hasSensors) {
                asteroid.GetComponent<SpriteRenderer>().color = asteroid.GetComponent<AsteroidInfo>().hasSensorColor;
            } else {
                asteroid.GetComponent<SpriteRenderer>().color = asteroid.GetComponent<AsteroidInfo>().noSensorColor;
            }

            // destroy item icons
            if (asteroid.GetComponent<AsteroidInfo>().greenPlantCount > 0 || asteroid.GetComponent<AsteroidInfo>().bluePlantCount > 0 
				|| asteroid.GetComponent<AsteroidInfo>().yellowPlantCount > 0 || asteroid.GetComponent<AsteroidInfo>().redPlantCount > 0) {
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

        //remove sprites on ScrapCloudCores
        foreach (GameObject cloud in theScrapClouds) {
            cloud.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

	public void SwapInvertScroll () {
		swapScroll = !swapScroll;
	}

	public void SetScrollSpeed (float val) {
		scrollSpeed = val;
	}
}