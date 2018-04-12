using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubState : MonoBehaviour {

	private Transform gravPoints;
	private Transform g1, g2, g3;

	// Use this for initialization
	void Awake () {
		gravPoints = new GameObject ("GravPoints").transform;
		gravPoints.SetParent (transform);
		g1 = new GameObject ("G1").transform;
		g1.SetParent (gravPoints);
		g1.position = Vector3.up * 2f;

		g2 = new GameObject ("G2").transform;
		g2.SetParent (gravPoints);
		g2.position = (Vector3.left * 2 + Vector3.down).normalized * 2f;

		g3 = new GameObject ("G3").transform;
		g3.SetParent (gravPoints);
		g3.position = (Vector3.right * 2 + Vector3.down).normalized * 2f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AssignPoint(Transform fragment) {
		if (g1.childCount == 0) {
			fragment.SetParent (g1);
		} else if (g2.childCount == 0) {
			fragment.SetParent (g2);
		} else if (g3.childCount == 0) {
			fragment.SetParent (g3);
		} else {
			return;
		}

		//fragment.localPosition = Vector3.zero;
	}
}
