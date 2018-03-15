using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HubPointer : MonoBehaviour {

	private GameObject hub;
	private Image myImage;

	[SerializeField]
	private float skin = 40f;
	private float tinyAmount = 0.0001f; // I slope the vertical walls of the screen a bit so that the slope is not infinity when computing the intersection point
	private Vector2 topLeftCorner;
	private Vector2 topRightCorner;
	private Vector2 bottomLeftCorner;
	private Vector2 bottomRightCorner;
	private float screenAspect;

	private Vector2 currentCorner1;
	private Vector2 currentCorner2;

	// Use this for initialization
	void Start () {
		hub = GameObject.FindGameObjectWithTag ("Hub");
		myImage = GetComponent<Image> ();
		topLeftCorner = new Vector2 (skin + tinyAmount, Screen.height - skin);
		topRightCorner = new Vector2 (Screen.width - skin + tinyAmount, Screen.height - skin);
		bottomLeftCorner = new Vector2 (skin, skin);
		bottomRightCorner = new Vector2 (Screen.width - skin, skin);
		currentCorner1 = topLeftCorner;
		currentCorner2 = topRightCorner;
		screenAspect = Mathf.Abs(topRightCorner.y / topRightCorner.x);
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

	//I'm sure there is a built in Unity thing which does this same functionality in a less math-intensive way, but I'm lazy
	//If you can find it and it performs better then you can replace
	//
	//Basically the idea is to draw a line from the center of the screen to the hub, and wherever that intersects with a line near the edge of the screen move the pointer there.
	Vector3 LineLineIntersection (Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2){
		float a = start1.x;
		float b = start1.y;
		float c = end1.x;
		float d = end1.y;
		float f = start2.x;
		float g = start2.y;
		float h = end2.x;
		float i = end2.y;

		float m1 = (d - b) / (c - a);
		float m2 = (i - g) / (h - f);
		float resultX = (m2*f - m1*a + b - g)/(m2-m1);
		float resultY = m1*(resultX - a) + b;
//		print (m1 + " " + m2);
		return new Vector3 (resultX, resultY);
	}

	//Move the pointer to (near) the edge of the screen in the direction of the hub
	//and angle it toward the hub
	void AimForHub () {
//		float x, y;
		Vector2 targPos = Camera.main.WorldToScreenPoint (hub.transform.position);
		Vector2 screenCenter = new Vector2 (Screen.width / 2, Screen.height / 2);
		float hubAspect = Mathf.Abs((targPos.y - screenCenter.y) / (targPos.x - screenCenter.x));

//		print ("hubAspect: " + hubAspect);
		if (hubAspect > screenAspect) {
			if (targPos.y < 0) {
				currentCorner1 = bottomLeftCorner;
				currentCorner2 = bottomRightCorner;
			} else {
				currentCorner1 = topLeftCorner;
				currentCorner2 = topRightCorner;
			}
		} else {
			if (targPos.x > 0) {
				currentCorner1 = topRightCorner;
				currentCorner2 = bottomRightCorner;
			} else {
				currentCorner1 = topLeftCorner;
				currentCorner2 = bottomLeftCorner;
			}
		}
//		print (currentCorner1 + " " + currentCorner2);

		Vector3 pointerPos = LineLineIntersection (targPos, screenCenter, currentCorner1, currentCorner2);

//		transform.LookAt (hub.transform.position);

		float angle = Vector2.Angle (Vector2.up, targPos - (Vector2)pointerPos);

		if (targPos.x > 0) {
			angle = 360 - angle;
		}

		transform.eulerAngles = Vector3.forward * (angle - 90);
		//transform.rotation = Quaternion.LookRotation((Vector3)targPos - targ);

		transform.position = pointerPos;
	}
}
