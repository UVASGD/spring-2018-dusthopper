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

	private int asteroidNum = 0;

	//Movement reference variables
	private Vector2 targRotDir;
	private Vector3 lastPos;

	[Header("Movement Options")]
	[SerializeField][Range(0f, 10f)] private float speed = 5;
	[SerializeField][Range(1f, 20f)] private float rotationSpeed = 10;

	[Header("Prefabs")]
	public AudioSource jumpSound;
	public Transform animPrefab;
	private UpgradeManager upgradeMgr;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		upgradeMgr = this.gameObject.GetComponent<UpgradeManager>();
		rb = GetComponent<Rigidbody2D> ();
		GameState.asteroid = GameObject.FindWithTag ("Hub").transform;
		transform.position = GameState.asteroid.position;
		lastPos = transform.position;
	}

	//This is just to control the "Switch Asteroid" debug button in the bottom of the screen

	public void GoToHUB()
	{
		SwitchAsteroid (GameObject.FindGameObjectWithTag ("Hub").transform);
	}

	public void GoToGravityFragment()
	{
		SwitchAsteroid (GameObject.Find("Asteroid Container").transform.GetChild(GameObject.Find("Asteroid Container").transform.childCount-Random.Range(1, 4)));
	}
	
	// Update is called once per frame
	void Update () {
		//Input direction. Use this variable so you don't call GetAxis multiple times a frame (faster)
		Vector2 inputVector = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical")).normalized; 

		//Player translational velocity vector
		Vector2 targVel = inputVector * (speed * upgradeMgr.walkSpeedMod); 

		//This section handles rotation lerping
		//Works by slowing moving point to look at around in unit circle around player. Player looks at the point exactly each frame
		targRotDir += inputVector * rotationSpeed * Time.deltaTime;
		targRotDir = Vector2.ClampMagnitude (targRotDir, 1f);
		float targRot = Mathf.Atan2 (targRotDir.x, -targRotDir.y) * Mathf.Rad2Deg;

		//If the player isn't moving or the map is open, stop all movement, otherwise move appropriately
		if (GameState.mapOpen || inputVector == Vector2.zero) {
			targVel = Vector3.zero;
		} else {
			rb.MoveRotation(targRot); //Directly set player rotation to the appropriate angle (this is a hard set, the lerping happens in targRot)

			//If the player is holding an object, keep the object oriented upwards
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

	//Called any time the player jumps to a new asteroid. 
	//If 'isAsteroid' is set to false, then it is implied that the jump failed ,and the player goes to a point in space and dies
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
