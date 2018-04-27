using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLimit : MonoBehaviour {

	public float MaxSpeed;
	private Rigidbody2D rb;

	void Start(){
		rb = GetComponent<Rigidbody2D> ();
	}
	// Update is called once per frame
	void Update () {
		if (rb.velocity.sqrMagnitude > MaxSpeed*MaxSpeed) {
			rb.velocity = rb.velocity.normalized * MaxSpeed;
		}
	}
}
