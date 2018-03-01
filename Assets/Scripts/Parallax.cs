using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finished
public class Parallax : MonoBehaviour {

	private float weight;


	// Use this for initialization
	void Start () {
		weight = 1 / (5 * transform.position.z + 1);
	}
	
	// Update is called once per frame
	void Update () {

		//transform.position = new Vector3 (weight * Camera.main.transform.position.x, weight * Camera.main.transform.position.y, transform.position.z);
		GetComponent<Renderer>().material.mainTextureOffset = new Vector2 (weight * Camera.main.transform.position.x, weight * Camera.main.transform.position.y);
	}
}
