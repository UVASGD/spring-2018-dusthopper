using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainFragment : MonoBehaviour {

	public float degreesPerSecond = 20f;
	private float rotSpeed;
	private Vector3 velocity;
	private Vector3 vel2;
    public GameObject pointer;

	private Transform hub;

	public State state;

	// Use this for initialization
	void Start () {
		state = State.initial;
		rotSpeed = degreesPerSecond;
        pointer = GameObject.Find(transform.parent.name.Replace("Asteroid", "Pointer"));
		hub = GameObject.FindWithTag ("Hub").transform;
	}
	
	// Update is called once per frame
	void Update () {
//		print (state);
		switch (state) {
		default:
			break;
		case State.initial:
			transform.eulerAngles += new Vector3 (0f, 0f, rotSpeed * Time.deltaTime);
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
			break;
		case State.final:
			transform.localPosition = Vector3.SmoothDamp (transform.localPosition, Vector3.zero, ref vel2, 1f);

			if (transform.localPosition.magnitude < 0.01f) {
				transform.localPosition = Vector3.zero;
				state = State.hehexd;
			}
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			GameState.RefreshHunger ();
			GameObject.Find("GM").transform.Find("SFX").Find("ObjectiveSFX").GetComponent<AudioSource>().Play();
			GameObject.Find("GM").transform.Find("SFX").Find("Music").GetComponent<AudioSource>().PlayDelayed(10f);
			state = State.transition;
			GetComponent<Collider2D> ().enabled = false;
			GameState.gravityFragmentCount += 1;
			GameObject.Find ("GM").GetComponent<EndGame> ().EndIfAble ();
            Destroy(pointer);
			//Destroy (gameObject);
		}
	}

	public enum State { initial, transition, hub, final, hehexd}
}
