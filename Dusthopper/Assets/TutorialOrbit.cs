using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOrbit : MonoBehaviour {

	public float speed = 5f;
	Transform target;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Hub").transform;
		transform.SetParent (target);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.Cross ((target.position - transform.position).normalized, Vector3.forward) * speed * Time.deltaTime;
	}
}
