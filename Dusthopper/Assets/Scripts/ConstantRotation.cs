using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour {

	public float degreesPerSecond = 20f;

	// Update is called once per frame
	void Update () {
		transform.eulerAngles += new Vector3 (0f, 0f, degreesPerSecond * Time.deltaTime);
	}
}
