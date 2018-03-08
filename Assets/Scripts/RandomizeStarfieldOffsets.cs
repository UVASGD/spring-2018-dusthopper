using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeStarfieldOffsets : MonoBehaviour {
	//This just shifts the starfield layers off by a random amount so that the player doesn't see the same starfield every time.
	void Start () {
		foreach (Transform child in transform) {
			child.position = new Vector3(Random.Range (-20f, 20f),Random.Range(-20f,20f),child.position.z);
		}
	}
}
