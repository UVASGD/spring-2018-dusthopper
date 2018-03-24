using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainFragment : MonoBehaviour {

	public float degreesPerSecond = 20f;
	private float rotSpeed;
	private Vector3 velocity;
    public GameObject pointer;

	public State state;

	// Use this for initialization
	void Start () {
		state = State.initial;
		rotSpeed = degreesPerSecond;
        pointer = GameObject.Find(transform.parent.name.Replace("Asteroid", "Pointer"));
	}
	
	// Update is called once per frame
	void Update () {
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
			transform.position = Vector3.SmoothDamp (transform.position, GameObject.FindGameObjectWithTag ("Hub").transform.position, ref velocity, 13f);

			if ((transform.position - GameObject.FindGameObjectWithTag ("Hub").transform.position).sqrMagnitude < 0.01f) {
				state = State.hub;
			}
			break;
		case State.hub:
			state = State.final;
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			GameState.RefreshHunger ();
			GameObject.Find("GM").transform.Find("SFX").Find("ObjectiveSFX").GetComponent<AudioSource>().Play();
			GameObject.Find("GM").transform.Find("SFX").Find("Music").GetComponent<AudioSource>().PlayDelayed(10f);
			state = State.transition;
            Destroy(pointer);
			//Destroy (gameObject);
		}
	}

	public enum State { initial, transition, hub, final}
}
