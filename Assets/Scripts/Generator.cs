using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

	public GameObject circle;
	public int quantity = 200;
	public float radius = 50;
	public float maxSpeed = 5;


	// Use this for initialization
	void Awake () {
		for (int i = 0; i < quantity; i++) {
			Vector3 pos = Random.insideUnitCircle * radius;

			GameObject inst = Instantiate (circle, pos, Quaternion.identity) as GameObject;
			inst.GetComponent<Rigidbody2D> ().velocity = Random.insideUnitCircle * maxSpeed;
			inst.name = "Asteroid" + i.ToString ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
