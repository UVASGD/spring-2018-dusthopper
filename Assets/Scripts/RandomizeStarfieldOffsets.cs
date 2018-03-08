using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeStarfieldOffsets : MonoBehaviour {
	void Start () {
		foreach (Transform child in transform) {
			child.position = new Vector3(Random.Range (-20f, 20f),Random.Range(-20f,20f),child.position.z);
		}
	}
}
