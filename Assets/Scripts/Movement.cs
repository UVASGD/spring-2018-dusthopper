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
	public AudioSource asrc;
	public AudioClip jumpSound;
	private Rigidbody2D rb;
	//public Transform asteroid;
	private Vector3 lastPos;
	[SerializeField] private float skin = 0.1f;
	[SerializeField] private Transform animPrefab;

	private int asteroidNum = 0;
	private UpgradeManager upgradeMgr;

	// Use this for initialization
	void Start () {
		upgradeMgr = this.gameObject.GetComponent<UpgradeManager>();
		rb = GetComponent<Rigidbody2D> ();
		GameState.asteroid = GameObject.FindWithTag ("Asteroid").transform;
		transform.position = GameState.asteroid.position;
		lastPos = transform.position;
	}

	//This is just to control the "Switch Asteroid" debug button in the bottom of the screen
	void OnGUI() {
		if (GUI.Button(new Rect(10, Screen.height - 40, 120, 30), "Switch Asteroid")) {
			ChangeAsteroid ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 targVel = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized * (speed * upgradeMgr.walkSpeedMod);

		if (GameState.mapOpen || (Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0)) {
			targVel = Vector3.zero;
		}

		//Keep constrained on current asteroidj
		if ((((Vector2)transform.position + targVel * Time.deltaTime) - (Vector2)GameState.asteroid.position + GameState.asteroid.GetComponent<Rigidbody2D>().velocity * Time.deltaTime).magnitude < GameState.asteroid.localScale.x / 2 - skin) {
			rb.velocity = targVel;
		} else {
			rb.velocity = Vector2.zero;
		}

		transform.position += GameState.asteroid.position - lastPos;
		lastPos = GameState.asteroid.position;

	}

	public void SwitchAsteroid (Transform a) {
		if (a != GameState.asteroid) {//shouldn't be able to jump to yourself
//		print ("Instantiating!");
			Transform inst = Instantiate (animPrefab, transform.position, transform.rotation);
			inst.GetComponent<JumpAnimation> ().origin = transform;
			inst.GetComponent<JumpAnimation> ().destination = a;
			asrc.PlayOneShot (jumpSound, 0.4f);
			transform.position = GameState.asteroid.position;
			GameState.asteroid = a;
			GameState.hasSensors = a.GetComponent<AsteroidSensorInfo> ().hasSensors;
			GameState.sensorRange = a.GetComponent<AsteroidSensorInfo> ().sensorRange;
			GameState.sensorTimeRange = a.GetComponent<AsteroidSensorInfo> ().sensorTimeRange;
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
			return;
		}
		//transform.position = GameState.asteroid.position;

		asteroidNum = (asteroidNum + 1) % GameObject.FindGameObjectsWithTag ("Asteroid").Length;

		SwitchAsteroid(GameObject.FindGameObjectsWithTag ("Asteroid") [asteroidNum].transform);
	}
}
