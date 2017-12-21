using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {

	[SerializeField][Range(0f, 10f)] private float speed = 5;

	private Rigidbody2D rb;
	//public Transform asteroid;
	private Vector3 lastPos;
	[SerializeField] private float skin = 0.1f;
	[SerializeField] private Transform animPrefab;

	private int asteroidNum = 0;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		GameState.asteroid = GameObject.FindWithTag ("Asteroid").transform;
		transform.position = GameState.asteroid.position;
		lastPos = transform.position;
	}

	void OnGUI() {
		if (GUI.Button(new Rect(10, Screen.height - 40, 120, 30), "Switch Asteroid")) {
			ChangeAsteroid ();
		}

//		print (GetComponent<Rigidbody2D> ().velocity);
	}

	/*void FixedUpdate () {
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Collider2D hit = Physics2D.OverlapPoint (mousePos);

			if (hit != null && hit.tag == "Asteroid" && (hit.transform.position - transform.position).sqrMagnitude < GameState.maxAsteroidDistance * GameState.maxAsteroidDistance) SwitchAsteroid (hit.transform);
		}
	}*/
	
	// Update is called once per frame
	void Update () {
		Vector2 targVel = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized * speed;

		if ((Input.GetAxisRaw ("Horizontal") == 0 && Input.GetAxisRaw ("Vertical") == 0) || GameState.mapOpen) {
			targVel = Vector3.zero;
		}

		//Keep constrained on current asteroid
		//print (asteroid.GetComponent<Rigidbody2D> ().velocity);
		if ((((Vector2)transform.position + targVel * Time.deltaTime) - (Vector2)GameState.asteroid.position + GameState.asteroid.GetComponent<Rigidbody2D>().velocity * Time.deltaTime).magnitude < GameState.asteroid.localScale.x / 2 - skin) {
			rb.velocity = targVel;
		} else {
			rb.velocity = Vector2.zero;
		}

		transform.position += GameState.asteroid.position - lastPos;
		lastPos = GameState.asteroid.position;

	}

	public void SwitchAsteroid (Transform a) {
//		print ("Instantiating!");
		Transform inst = Instantiate (animPrefab, transform.position, transform.rotation);
		inst.GetComponent<JumpAnimation> ().origin = transform;
		inst.GetComponent<JumpAnimation> ().destination = a;
		//inst.GetComponent<JumpAnimation> ().Animate ();
		transform.position = GameState.asteroid.position;
		GameState.asteroid = a;
		GameState.hasSensors = a.GetComponent<AsteroidSensorInfo> ().hasSensors;
		GameState.sensorRange = a.GetComponent<AsteroidSensorInfo> ().sensorRange;
		GameState.sensorTimeRange = a.GetComponent<AsteroidSensorInfo> ().sensorTimeRange;

//		print (GameState.hasSensors);
//		print (GameState.sensorRange);
//		print (GameState.sensorTimeRange);
	}

	public void ChangeAsteroid () {
		if (GameState.asteroid == null) {
			return;
		}
		//transform.position = GameState.asteroid.position;

		asteroidNum = (asteroidNum + 1) % GameObject.FindGameObjectsWithTag ("Asteroid").Length;

		SwitchAsteroid(GameObject.FindGameObjectsWithTag ("Asteroid") [asteroidNum].transform);
	}
}
