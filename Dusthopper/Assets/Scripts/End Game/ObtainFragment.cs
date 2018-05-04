using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainFragment : MonoBehaviour {

	public static int fragmentCount = 0;

	public int fragmentID;

	public float degreesPerSecond = 20f;
	private float rotSpeed;
	private Vector3 velocity;
	private Vector3 vel2;
    public GameObject pointer;

	private Transform hub;

	public State state;

	private Transform gravFragAsteroid;
	private Vector3 randomOffset;

	// Use this for initialization
	void Start () {
		state = State.initial;
		rotSpeed = degreesPerSecond;
		gravFragAsteroid = transform.parent;
        pointer = GameObject.Find(transform.parent.name.Replace("Asteroid", "Pointer"));
		hub = GameObject.FindWithTag ("Hub").transform;

		fragmentID = fragmentCount++;

//		if (GameState.obtainedFragment [fragmentID] == true) {
//			print ("YARRRR Gravity Fragment " + fragmentID + " obtained: true");
//			hub.GetComponent<HubState> ().AssignPoint (transform);
//			state = State.hehexd;
//		}
//		print ("FragmentID: " + fragmentID);

		if (fragmentCount > 2) {
			fragmentCount = 0;
		}

		if (GameState.obtainedFragment [fragmentID]) {
			hub.GetComponent<HubState> ().AssignPoint (transform);
			state = State.hehexd;
			Destroy(pointer);
		}

		randomOffset = (Vector3)(Random.insideUnitCircle.normalized) * 2;
//		Transform asteroidContainer = GameObject.Find ("Asteroid Container").transform;
//		gravFragAsteroid = asteroidContainer.GetChild (asteroidContainer.childCount - 1 - fragmentID);
	}
	
	// Update is called once per frame
	void Update () {
		print (state);
		switch (state) {
		default:
			transform.eulerAngles += new Vector3 (0f, 0f, -rotSpeed * Time.deltaTime);
			break;
		case State.initial:
			transform.eulerAngles += new Vector3 (0f, 0f, rotSpeed * Time.deltaTime);
			transform.position = gravFragAsteroid.position + randomOffset;
			break;
		case State.transition:
			transform.eulerAngles += new Vector3 (0f, 0f, rotSpeed * Time.deltaTime);
			//if (rotSpeed < degreesPerSecond * 10)
			rotSpeed += 360 * Time.deltaTime;
			if (transform.parent)
				transform.SetParent (null);
			transform.position = Vector3.SmoothDamp (transform.position, hub.position, ref velocity, (GameState.endGame ? 5f : 13f));

			if ((transform.position - hub.position).magnitude < 10f) {
				state = State.hub;
			}
			break;
		case State.hub:
			state = State.final;
			hub.GetComponent<HubState> ().AssignPoint (transform);
			transform.GetChild (0).gameObject.SetActive (false);
			break;
		case State.final:
			transform.eulerAngles += new Vector3 (0f, 0f, rotSpeed * Time.deltaTime);
			transform.localPosition = Vector3.SmoothDamp (transform.localPosition, Vector3.zero, ref vel2, 1f);
			transform.GetChild (0).gameObject.SetActive (false);
			if (transform.localPosition.magnitude < 0.01f) {
				transform.localPosition = Vector3.zero;
				state = State.hehexd;
			}
			break;
		case State.hehexd:
			//print ("HEHEXD");
			transform.eulerAngles += new Vector3 (0f, 0f, rotSpeed * Time.deltaTime);
			transform.localPosition = Vector3.zero;
			transform.GetChild (0).gameObject.SetActive (false);
			GetComponent<Collider2D> ().enabled = false;
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			GameState.RefreshHunger ();
			GameState.obtainedFragment [fragmentID] = true;
			GameState.SaveGame ();
			GameObject.Find("GM").transform.Find("SFX").Find("ObjectiveSFX").GetComponent<AudioSource>().Play();
			GameObject.Find("GM").transform.Find("SFX").Find("Music").GetComponent<AudioSource>().PlayDelayed(10f);
			state = State.transition;
			GetComponent<Collider2D> ().enabled = false;
			GameState.gravityFragmentCount++;
			GameObject.Find ("GM").GetComponent<EndGame> ().EndIfAble ();
            Destroy(pointer);
			//Destroy (gameObject);
		}
	}

	public enum State { initial, transition, hub, final, hehexd}
}
