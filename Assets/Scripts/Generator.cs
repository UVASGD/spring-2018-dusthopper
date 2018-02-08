using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {
	//This populates the world with asteroids. 
	//TODO: Every special asteroid and object generation thing will eventually have to be put here

	public GameObject circle; //"circle" is the default Asteroid Prefab. Not a very good name but it's not final by any means. TODO: Make this a list of prefabs
	public GameObject container; // Newly created asteroids will be children of this object. For Hierarchy organization. Potential TODO: Create in script as opposed to using reference
	public int quantity = 200; //how many asteroids?
	public float radius = 200; //how far away from the center can they spawn?
	public float maxSpeed = 5; //how fast can they be going at the start?

	//proc gen sensor stuff
	public float sensorChance;
	public int avgSensorRange;
	public int sensorRangeRange;
	public int avgSensorTimeRange;
	public int sensorTimeRangeRange;
	public Color hasSensorColor;



	// Use this for initialization
	void Awake (){
		//Generate Asteroids TODO: Put this in Generate() function
		for (int i = 0; i < quantity; i++) {
			Vector3 pos = Random.insideUnitCircle * radius;

			GameObject inst = Instantiate (circle, pos, Quaternion.identity) as GameObject;
			inst.transform.parent = container.transform;
//			inst.GetComponent<Rigidbody2D> ().angularVelocity = Random.Range (0f, 50f);
			inst.name = "Asteroid" + i.ToString ();
			inst.GetComponent<Rigidbody2D> ().velocity = Random.insideUnitCircle * maxSpeed;
			if (Random.value <= sensorChance) {
				inst.GetComponent<AsteroidSensorInfo> ().hasSensors = true;
				inst.GetComponent<AsteroidSensorInfo> ().sensorRange = Random.Range (avgSensorRange - sensorRangeRange, avgSensorRange + sensorRangeRange);
				inst.GetComponent<AsteroidSensorInfo> ().sensorTimeRange = Random.Range (avgSensorTimeRange - sensorTimeRangeRange, avgSensorRange + sensorTimeRangeRange);
				inst.GetComponent<SpriteRenderer>().color = hasSensorColor;
			} else {
				inst.GetComponent<AsteroidSensorInfo> ().hasSensors = false;
			}
		}
	}
}
