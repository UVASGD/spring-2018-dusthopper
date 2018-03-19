using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MaintainVelocity : MonoBehaviour {

	public Vector2 speedRange = new Vector2 (0.2f, 2f);
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameState.asteroid == transform) {
			print ("Velocity: " + rb.velocity + "\tSpeed: " + rb.velocity.magnitude);
		}
		if (rb.velocity.sqrMagnitude < speedRange.x * speedRange.x) {
			rb.AddForce (rb.velocity.normalized * 1f);
		} else if (rb.velocity.sqrMagnitude > speedRange.y * speedRange.y) {
			rb.AddForce (rb.velocity.normalized * -1f);
		}
	}
}
