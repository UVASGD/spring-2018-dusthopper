using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: figure out situations where the player places a jump but there would not be time to charge it up.
//Currently the player is blocked from doing this, but possibly let them fail? Possibly alert them to why their jump is not being placed?
public class PathMaker : MonoBehaviour {
	//This script maintains the list of jumps the player has scheduled.
	//To do that it also:
	//		Plays a charge jump sound effect when a scheduled jump is imminent
	// 		Disables manual jumps when a scheduled jump is imminent
	// 		Calls SwitchAsteroid in the movement script when the jump is supposed to happen
	// 		Updates the scheduled path lines in the map
	// 		Allows the player to add / remove jumps from the schedule by clicking on asteroids.


	public SortedList<float,Transform> path; //key = time to start charging jump , value = transform of target asteroid
	public SortedList<float,float> jumpTimes; //key and value are both the time the jump will actually take place
	public GameObject container;
	private GameObject player;
	public AudioSource chargeJump;
	public AudioSource jumpTooFar;
	private bool mapOpenLF;
	private List<GameObject> lines;
	public float percentIdle;//jumps not currently active should be still dimly lit so the player can see the long term plan. This is the alpha value for those jumps.

//	public Animation 
	[HideInInspector]
	public float initialTime;
	private float timeOfJump;
	private float timeToStartCharging;
	private float timeSinceChargingStarted;
	private float GameStateTimeLF;
	private bool gamePausedLF;
	public float tolerance; //When final jump is made, I want to be a bit lenient with max distance because of various ways the jump may have changed since being scheduled.

	public bool jumpTimeElapsed = false;
    public Text failureText1;

    //During whether or not the game should fastforward time during jump scheduling
    public bool autoScroll = true;

	public float highlightAmount = 0.5f;

	public bool specialGrayPollenJump = false;

	public bool tutorialAllows;

    // Use this for initialization
    void Start () {
		path = new SortedList<float,Transform> (0);
		jumpTimes = new SortedList<float, float> (0);
		player = GameState.player;
		lines = new List<GameObject> ();
		GameStateTimeLF = 0f;
		timeSinceChargingStarted = 0f;
		tutorialAllows = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.mapOpen) {
			EditPath ();
			if (!mapOpenLF) {
				if (chargeJump.isPlaying) {
					chargeJump.Pause ();
				}

				foreach (GameObject line in lines) {
					line.GetComponent<LineRenderer> ().enabled = true;
				}
			}
		} else {
			TraversePath ();

			if (GameState.gamePaused && !gamePausedLF) {
				if (chargeJump.isPlaying) {
					chargeJump.Pause ();
				}
			} else if (!GameState.gamePaused && gamePausedLF) {
				if (!chargeJump.isPlaying) {
					chargeJump.UnPause ();
				}
			}

			if (mapOpenLF) {
				if (chargeJump.time > 0) {
					chargeJump.UnPause ();
				}

				foreach (GameObject line in lines) {
					line.GetComponent<LineRenderer> ().enabled = false;
				}
			}
		}

		//update display
		if (lines.Count > 0) {
			lines[0].GetComponent<LineRenderer> ().SetPosition (0, new Vector3 (GameState.asteroid.position.x, GameState.asteroid.position.y, 5f));
			lines[0].GetComponent<LineRenderer> ().SetPosition (1, new Vector3 (path.Values[0].position.x, path.Values[0].position.y, 5f));
			float a = getAlpha (GetComponent<TimeManipulator> ().timeFromNow + initialTime, 0);
			lines[0].GetComponent<LineRenderer> ().startColor = new Color(0.5f*a,0f,0.5f*a,a);
			lines[0].GetComponent<LineRenderer> ().endColor = new Color(1f,0.69f,0f,a);
			for(int i = 1; i < path.Count; i++){
				lines[i].GetComponent<LineRenderer> ().SetPosition (0, new Vector3 (path.Values[i-1].position.x, path.Values[i-1].position.y, 5f));
				lines[i].GetComponent<LineRenderer> ().SetPosition (1, new Vector3 (path.Values[i].position.x, path.Values[i].position.y, 5f));
				a = getAlpha (GetComponent<TimeManipulator> ().timeFromNow + initialTime, i);
				lines[i].GetComponent<LineRenderer> ().startColor = new Color(0.5f*a,0f,0.5f*a,a);
				lines[i].GetComponent<LineRenderer> ().endColor = new Color(1f,0.69f,0f,a);
			}
			
		}
		mapOpenLF = GameState.mapOpen;
		gamePausedLF = GameState.gamePaused;
	}

	float getAlpha(float time, int pathIndex){
		float chargeTime = path.Keys [pathIndex];
		float jumpTime = jumpTimes.Keys [pathIndex];
		if (time >= chargeTime && time <= jumpTime) {
			return ((1 - percentIdle) / GameState.secondsPerJump) * (time - jumpTime) + 1;
		}
		if(time >= jumpTime && time <= jumpTime + GameState.secondsPerJump){
			return  ((percentIdle - 1) / GameState.secondsPerJump) * (time - jumpTime) + 1;
		}
		return percentIdle;
	}

	void EditPath () {
		if (Input.GetMouseButtonDown(0) && tutorialAllows)
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Collider2D hit = Physics2D.OverlapPoint (mousePos);

			if (hit != null && hit.gameObject.layer == LayerMask.NameToLayer("Asteroid")) {
				//Try to find the jump in the current path
				bool foundIt = false;
				int i = 0;
				int prevAsteroidIndex = -1;
				while (i < path.Keys.Count && !foundIt) {
					if (jumpTimes.Keys [i] < GetComponent<TimeManipulator> ().timeFromNow + initialTime) {
						prevAsteroidIndex = i;
					}
					if (hit.transform == path.Values[i]) {
						foundIt = true;
						print ("removing jump to asteroid " + hit.transform.gameObject.name + " at time " + jumpTimes.Keys [i]);
						for (int j = path.Count - 1; j >= i; j--) {
							// remove highlights
							if (path.Values[j].gameObject.GetComponent<AsteroidInfo> ().hasSensors) {
								path.Values[j].gameObject.GetComponent<SpriteRenderer> ().color = path.Values[j].gameObject.GetComponent<AsteroidInfo> ().iconWithSensor;
							} else {
								path.Values[j].gameObject.GetComponent<SpriteRenderer> ().color = path.Values[j].gameObject.GetComponent<AsteroidInfo> ().iconWithoutSensor;
							}
							path.RemoveAt (j);
							jumpTimes.RemoveAt (j);
							Destroy (lines [j]);
							lines.RemoveAt (j);
						}
					}
					i++;
				}
				if (!foundIt) {//If you didn't find a jump to remove, the player is trying to add a jump
					Transform prevAsteroid;
					if (prevAsteroidIndex == -1) {
						prevAsteroid = GameState.asteroid;
					} else {
						prevAsteroid = path.Values [prevAsteroidIndex];
					}
					if (prevAsteroid != hit.transform) {//don't let player add jumps to current asteroid
						timeOfJump = GetComponent<TimeManipulator> ().timeFromNow + initialTime;
						timeToStartCharging = timeOfJump - GameState.secondsPerJump;
//						print("new jump adding");
//						print ("timeOfJump: " + timeOfJump);
//						print ("timeToStartCharging: " + timeToStartCharging);
//						print ("initialTime: " + initialTime);
						//Is it a valid jump?
						if (timeToStartCharging >= initialTime) {
							bool overlap = false;
							i = 0;
							while (i < path.Keys.Count && !overlap) {
//								print ("check: " + Mathf.Abs (path.Keys [i] - timeToStartCharging));
								if (Mathf.Abs (path.Keys [i] - timeToStartCharging) < GameState.secondsPerJump) {
									overlap = true;
                                    displayFailedJump("Jump overlaps with an existing jump");
									print ("jump not scheduled because it overlaps with an existing jump");
								}
								i++;
							}
							if (!overlap) {
//								print ("scheduled jump to asteroid " + hit.transform.gameObject.name + " at time " + timeOfJump);
								if (specialGrayPollenJump) {
									player.GetComponent<Movement> ().SwitchAsteroid (hit.gameObject.transform);
									specialGrayPollenJump = false;
									Camera.main.GetComponent<CameraScrollOut> ().closeMap ();
									return;
								}
								if(GameState.sensorTimeRange - GetComponent<TimeManipulator> ().timeFromNow >= GameState.secondsPerJump && autoScroll){
									GetComponent<TimeManipulator> ().AutoScroll ();
								}
								path.Add (timeToStartCharging, hit.transform);
								jumpTimes.Add (timeOfJump, timeOfJump);
								GameObject newLine = new GameObject ();
								newLine.name = "Line" + i.ToString ();
								newLine.transform.parent = container.transform;
								newLine.layer = LayerMask.NameToLayer ("UI");
								newLine.AddComponent (typeof(LineRenderer));
								newLine.GetComponent<LineRenderer> ().material = new Material (Shader.Find ("Sprites/Default"));
								newLine.GetComponent<LineRenderer> ().startColor = new Color (1f, 0.69f, 0f, 1);
								newLine.GetComponent<LineRenderer> ().endColor = new Color (1f, 0.69f, 0f, 1);
								newLine.GetComponent<LineRenderer> ().startWidth = 0.3f;
								newLine.GetComponent<LineRenderer> ().endWidth = 0.3f;
								newLine.GetComponent<LineRenderer> ().positionCount = 2;
								lines.Add (newLine);
								hit.GetComponent<SpriteRenderer> ().color = new Color (hit.GetComponent<SpriteRenderer> ().color.r + highlightAmount, hit.GetComponent<SpriteRenderer> ().color.g + highlightAmount, hit.GetComponent<SpriteRenderer> ().color.b + highlightAmount, 1);
							}
						} else {
							print ("jump not scheduled because you can't charge in time");
							displayFailedJump ("Not enough time to charge");
						}
					} else {
						print ("jump not scheduled because player tried to jump to the asteroid they'll be on");
						displayFailedJump ("You'll be on that asteroid already");
					}
				}
			}
		}
	}

	void TraversePath(){
		if (path.Count > 0 && GameState.time >= path.Keys [0] && path.Values [0] != GameState.asteroid) {
			if (timeSinceChargingStarted >= GameState.secondsPerJump) {
				if ((path.Values [0].position - player.transform.position).sqrMagnitude < (GameState.maxAsteroidDistance * GameState.maxAsteroidDistance + tolerance)) {
					print ("jumping to asteroid " + path.Values [0].gameObject.name + " at time " + GameState.time);
					player.GetComponent<Movement> ().SwitchAsteroid (path.Values [0]);
					chargeJump.time = 0f;
				} else {
					print ("jump cancelled - too far. clearing jumps");
					displayFailedJump ("jump too far");
					if (chargeJump.isPlaying && !FindObjectOfType<ManualJump>().manuallyJumping) {
						chargeJump.Stop ();
					}
					jumpTooFar.Play ();
					RemoveJumps ();
				}
				path.RemoveAt (0);
				jumpTimes.RemoveAt (0);
				Destroy (lines [0]);
				lines.RemoveAt (0);
				timeSinceChargingStarted = 0f;
				GameState.manualJumpsDisabled = false;
			} else if (GameState.time != GameStateTimeLF) {
				timeSinceChargingStarted += Time.deltaTime;
				GameState.manualJumpsDisabled = true;
				if (!chargeJump.isPlaying) {
					chargeJump.Play ();
				}
			}
		} else if (path.Count == 0) {
			if (chargeJump.isPlaying && !FindObjectOfType<ManualJump>().manuallyJumping) {
				chargeJump.Stop ();
			}
		}
		GameStateTimeLF = GameState.time;
	}

    public void RemoveJumps()
    {
        //print ("removing lowest jump");

        path.Clear();
        jumpTimes.Clear();
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
        lines.Clear();
//        print("jump schedule cleared");
//		if (chargeJump.isPlaying) {
//			chargeJump.Stop ();
//		}
    }

    public void ToggleAutoScroll(){
        autoScroll = !autoScroll;
    }

    //this method will display the passed text in the failureText1 text object
    //this is primarily used for informing the player why they can't jump
    public void displayFailedJump(string text) {

        failureText1.text = text;
        failureText1.GetComponent<DecayUnscaled>().setOpaque();

    }
}
