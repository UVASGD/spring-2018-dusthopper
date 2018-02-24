﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidInfo : MonoBehaviour {
	//Contained by every asteroid instance
	//Holds some important things about this asteroid and how it appears in the map
	public bool hasSensors;
	public float sensorRange = 30f;
	public float sensorTimeRange = 30f;
	public Sprite mapIcon;
	[HideInInspector]
	public Sprite asteroidSprite;

	void Start() {
		asteroidSprite = GetComponent<SpriteRenderer> ().sprite;
	}
}