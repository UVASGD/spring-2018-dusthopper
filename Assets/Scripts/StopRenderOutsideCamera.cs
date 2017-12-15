using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class StopRenderOutsideCamera : MonoBehaviour {

//	Transform player;

	// Use this for initialization
	void Start () {
//		player = GameObject.FindWithTag ("Player").transform;
		InvokeRepeating ("UpdateDisplay", 0f, 0.5f);
	}
	
	public void UpdateDisplay () {
		if ((((Vector2)transform.position - (Vector2)Camera.main.transform.position).sqrMagnitude > 450)) {
			GetComponent<SpriteRenderer> ().enabled = false;
		} else {
			GetComponent<SpriteRenderer> ().enabled = true;
		}
	}
}
