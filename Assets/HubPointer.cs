using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubPointer : MonoBehaviour {

	private GameObject hub;
	private Image myImage;

	[SerializeField]
	private float skin = 40f;

	// Use this for initialization
	void Start () {
		hub = GameObject.FindGameObjectWithTag ("Hub");
		myImage = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 hubScreenPos = Camera.main.WorldToScreenPoint (hub.transform.position);
		if (hubScreenPos.x < -skin || hubScreenPos.x > Screen.width + skin || hubScreenPos.y < -skin || hubScreenPos.y > Screen.height + skin) {
			myImage.enabled = true;
			AimForHub ();
		} else {
			myImage.enabled = false;
		}
	}

	void AimForHub () {
		float x, y;
		Vector2 targPos = Camera.main.WorldToScreenPoint (hub.transform.position);

		if (targPos.x > Screen.width - skin) {
			x = Screen.width - skin;
		} else if (targPos.x < skin) {
			x = skin;
		} else {
			x = targPos.x;
		}
			
		if (targPos.y > Screen.height - skin) {
			y = Screen.height - skin;
		} else if (targPos.y < skin) {
			y = skin;
		} else {
			y = targPos.y;
		}

		Vector3 pointerPos = new Vector3 (x, y);

		//transform.LookAt (hub.transform.position);

		float angle = Vector2.Angle (Vector2.up, targPos - (Vector2)pointerPos);
		int quadrant;

		if (angle % 360 < 90) {
			quadrant = 2;
		} else if (angle % 360 < 180) {
			quadrant = 3;
		} else if (angle % 360 < 270) {
			quadrant = 4;
		} else {
			quadrant = 1;
		}

		print ("Angle: " + angle + "\tQuadrant: " + quadrant);

		if (targPos.x > 0) {
			angle = 360 - angle;
		}

		transform.eulerAngles = Vector3.forward * (angle - 90);
		//transform.rotation = Quaternion.LookRotation((Vector3)targPos - targ);

		transform.position = pointerPos;
	}
}
