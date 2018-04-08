using UnityEngine;
using System.Collections;

public class ShakeObject : MonoBehaviour {
	// Amplitude of the shake. A larger value shakes the camera harder.
	private float shakeAmount = 0.07f;

	Vector3 originalPos;

	void Start () {
		originalPos = transform.localPosition;
	}

	void Update () {
		transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
	}
}