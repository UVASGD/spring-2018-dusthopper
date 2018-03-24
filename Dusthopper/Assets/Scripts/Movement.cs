using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour {
	//This handles player movement, including:
	//		WASDing around on current asteroid 
	//		keeping movement restricted to radius away from the center of current asteroid
	// 		SwitchAsteroid, the method which actually switches asteroids to a target and updates some variables to do with that
	// 		Playing some sound effects

	[SerializeField][Range(0f, 10f)] private float speed = 5;
	public AudioSource jumpSound;
	private Rigidbody2D rb;
	//public Transform asteroid;
	private Vector3 lastPos;
	public Transform animPrefab;

	private int asteroidNum = 0;
	private UpgradeManager upgradeMgr;

	private float rotVel;

	// Use this for initialization
	void Start () {
		upgradeMgr = this.gameObject.GetComponent<UpgradeManager>();
		rb = GetComponent<Rigidbody2D> ();
		GameState.asteroid = GameObject.FindWithTag ("Hub").transform;
		transform.position = GameState.asteroid.position;
		lastPos = transform.position;
	}

	//This is just to control the "Switch Asteroid" debug button in the bottom of the screen
	void OnGUI() {
		if (!GameState.debugMode)
			return;
		
		if (GUI.Button(new Rect(10, Screen.height - 40, 120, 30), "Switch Asteroid")) {
			ChangeAsteroid ();
		}

		if (GUI.Button(new Rect(Screen.width - 130, Screen.height - 40, 120, 30), "Return to Hub")) {
			SwitchAsteroid (GameObject.FindGameObjectWithTag ("Hub").transform);
		}

		if (GUI.Button(new Rect(Screen.width - 130, Screen.height - 120, 120, 30), "Go To Gravity Fragment")) {
			SwitchAsteroid (GameObject.Find("Asteroid Container").transform.GetChild(GameObject.Find("Asteroid Container").transform.childCount-1));
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 targVel = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized * (speed * upgradeMgr.walkSpeedMod);
		float targRot = Mathf.Atan2 (Input.GetAxisRaw ("Vertical"), Input.GetAxisRaw ("Horizontal")) * Mathf.Rad2Deg + 90;

		if (Mathf.Abs(targRot - rb.rotation) > 180) {
			targRot -= 360f;
		}

		if (GameState.mapOpen || (Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0)) {
			targVel = Vector3.zero;
		} else {
			rb.rotation = Mathf.SmoothDamp (rb.rotation, targRot, ref rotVel, 0.1f);
			if (GetComponent<PlayerCollision> ().holding) {
				GetComponent<PlayerCollision> ().heldObject.transform.rotation = Quaternion.identity;
			}
		}

		//Stop following asteroid movement if there is none
		if (!GameState.asteroid)
			return;
		
		//Keep constrained on current asteroidj
		if ((((Vector2)transform.position + targVel * Time.deltaTime) - (Vector2)GameState.asteroid.position + GameState.asteroid.GetComponent<Rigidbody2D>().velocity * Time.deltaTime).magnitude < GameState.asteroid.GetComponent<AsteroidInfo>().radius) {
			rb.velocity = targVel;
		} else {
			rb.velocity = Vector2.zero;
		}

		transform.position += GameState.asteroid.position - lastPos;
		lastPos = GameState.asteroid.position;

	}

	public void SwitchAsteroid (Transform a, bool isAsteroid = true) {
		if (a != GameState.asteroid) {//shouldn't be able to jump to yourself
//		print ("Instantiating!");
			Transform inst = Instantiate (animPrefab, transform.position, transform.rotation);
			inst.GetComponent<JumpAnimation> ().origin = transform;
			inst.GetComponent<JumpAnimation> ().destination = a;

			/* W.I.P
			if(GameObject.FindWithTag ("Player").GetComponent<PlayerCollision> ().holding) {
				GameObject held = GameObject.FindWithTag ("Player").GetComponent<PlayerCollision> ().heldObject;
				Transform inst_held = Instantiate (animPrefab, transform.position, transform.rotation);
				inst_held.GetComponent<JumpAnimation> ().origin = transform;
				inst_held.GetComponent<JumpAnimation> ().destination = a;
				inst_held.GetComponent<SpriteRenderer> ().sprite = held.GetComponent<SpriteRenderer> ().sprite;
				inst_held.transform.localScale = held.transform.localScale;
				inst_held.GetComponent<SpriteRenderer> ().color = held.GetComponent<SpriteRenderer> ().color;
			}
			*/

			jumpSound.Play ();
			if (isAsteroid) {
				transform.position = GameState.asteroid.position;
			} else {
				transform.position = a.position;
			}

			//If the jump location is not an asteroid, don't change asteroid values
			if (!isAsteroid)
				return;

			if (GameState.asteroid.tag != "Hub" && a.tag == "Hub") {
				GameObject.FindObjectOfType<RunHandler> ().EndRun (true);
			} else if (GameState.asteroid.tag == "Hub" && a.tag != "Hub") {
				GameObject.FindObjectOfType<RunHandler> ().StartRun ();
			}

			GameState.asteroid = a;
			GameState.hasSensors = a.GetComponent<AsteroidInfo> ().hasSensors;
			GameState.sensorRange = a.GetComponent<AsteroidInfo> ().sensorRange;
			GameState.sensorTimeRange = a.GetComponent<AsteroidInfo> ().sensorTimeRange;
			if (GameState.hasSensors) {
				Camera.main.GetComponent<CameraScrollOut> ().jumpingToAsteroidWithMap = true;
			}
		}
//		print (GameState.hasSensors);
//		print (GameState.sensorRange);
//		print (GameState.sensorTimeRange);
	}

	//This is just used by the "Switch Asteroid" debug button in the bottom of the screen
	//SwitchAsteroid(Transform a) is the function you want to call to switch an asteroid.
	public void ChangeAsteroid () {
		if (GameState.asteroid == null) {
			Debug.LogError ("No current asteroid to swap from!");
			return;
		}
		//transform.position = GameState.asteroid.position;

		asteroidNum = (asteroidNum + 1) % GameObject.FindGameObjectsWithTag ("Asteroid").Length;

		SwitchAsteroid(GameObject.FindGameObjectsWithTag ("Asteroid") [asteroidNum].transform);
	}
}
