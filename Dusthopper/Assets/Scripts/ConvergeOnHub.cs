using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ConvergeOnHub : MonoBehaviour {

	private GameObject hub;
	private Rigidbody2D rb;

	private bool slowing = true;

	// Use this for initialization
	void Awake () {
		hub = GameObject.FindWithTag ("Hub");
		slowing = true;
		rb = GetComponent<Rigidbody2D> ();
		//rb.mass = 
	}
	
	// Update is called once per frame
	void Update () {
		if (!slowing) {
			rb.AddForce ((hub.transform.position - transform.position).normalized * 4);
		} else {
			if (rb.velocity.sqrMagnitude > 0.01f) {
				rb.velocity -= rb.velocity * Time.deltaTime;
			} else {
				slowing = false;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other != null) {
			if (other.collider.tag == "Hub") {
				if (GameState.asteroid == transform) {
					GameState.player.GetComponent<Movement> ().SwitchAsteroid (hub.transform);
				}
				hub.transform.localScale += Vector3.one * 0.1f;
				gameObject.SetActive (false);
			}
		}
	}
}
