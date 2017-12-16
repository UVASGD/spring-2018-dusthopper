﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPathLines : MonoBehaviour {

	public float maxDist = 20;

	private GameObject[] asteroidList;
	private GameObject[] lines;

	// Use this for initialization
	void Start () {
		asteroidList = GameObject.FindGameObjectsWithTag ("Asteroid");
		lines = new GameObject[asteroidList.Length * (asteroidList.Length - 1) / 2];
		for (int i = 0; i < lines.Length; i++) {
			lines [i] = new GameObject ();
			lines [i].name = "Line" + i.ToString ();
			lines [i].transform.parent = transform;
			lines [i].AddComponent (typeof(LineRenderer));
			lines [i].GetComponent<LineRenderer> ().material = new Material(Shader.Find ("Sprites/Default"));
			lines [i].GetComponent<LineRenderer> ().startColor = Color.green;
			lines [i].GetComponent<LineRenderer> ().endColor = Color.green;
			lines [i].GetComponent<LineRenderer> ().startWidth = 0.1f;
			lines [i].GetComponent<LineRenderer> ().endWidth = 0.1f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		DrawPaths ();
	}

	void DrawPaths () {
		int iter = 0;
		for (int i = 1; i < asteroidList.Length; i++) {
			for (int j = i; j < asteroidList.Length; j++) {
				if ((asteroidList [i - 1].transform.position - asteroidList [j].transform.position).sqrMagnitude < GameState.maxAsteroidDistance * GameState.maxAsteroidDistance) {
					lines [iter].GetComponent<LineRenderer> ().enabled = true;
					lines [iter].GetComponent<LineRenderer>().SetPosition (0, new Vector3(asteroidList [i - 1].transform.position.x, asteroidList [i - 1].transform.position.y, 10f));
					lines [iter].GetComponent<LineRenderer>().SetPosition (1, new Vector3(asteroidList [j].transform.position.x, asteroidList [j].transform.position.y, 10f));
				} else {
					lines [iter].GetComponent<LineRenderer> ().enabled = false;
				}

				if (!GameState.mapOpen) {
					lines [iter].GetComponent<LineRenderer> ().enabled = false;
				}
				iter++;
			}
		}
	}
}