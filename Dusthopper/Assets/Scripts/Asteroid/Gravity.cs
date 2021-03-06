﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gravitates things slightly to the hub based with mass and distance
public class Gravity : MonoBehaviour {

	private Rigidbody2D rb;
	Rigidbody2D otherRB;
	public LayerMask asteroidLayer;
	[Range(0f, 1000f)] public float multiplier = 5f;
	[Range(0f, 1000f)] public float range = 20f;
	private int layer;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		multiplier = 5f;
		layer = 1 << LayerMask.NameToLayer ("Asteroid");
		asteroidLayer.value = layer;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Collider2D[] closeAsteroids = Physics2D.OverlapCircleAll (transform.position, range, layer);

		foreach (var asteroid in closeAsteroids) {
			if (asteroid.tag != "Hub" && asteroid != GetComponent<Collider2D>()) {
				otherRB = asteroid.GetComponent<Rigidbody2D> ();
				if (rb && otherRB) {
					if (asteroid.gameObject.GetComponent<AsteroidInfo> ().pulledByGrav) {
						//print ("pulling asteroids");
					
						//print ("still pulling");
						Vector2 forceVector = transform.position - asteroid.transform.position;
						otherRB.AddForce (multiplier * forceVector.normalized * rb.mass * otherRB.mass / forceVector.sqrMagnitude);
					}
				}
			}
		}
	}
}
