using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TutorialKeyPress : MonoBehaviour {

	public KeyCode key1;
	public KeyCode key2;
	private SpriteRenderer myRenderer;

	// Use this for initialization
	void Start () {
		myRenderer = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (key1 == KeyCode.Mouse3) {
			if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
				myRenderer.enabled = true;
			} else {
				myRenderer.enabled = false;
			}
		} else {
			if (Input.GetKey (key1)) {
				myRenderer.enabled = true;
				//myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 1f);
			} else {
				myRenderer.enabled = false;
				//myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 0f);
			}
		}

		if (key2 == KeyCode.Mouse3) {
			if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
				myRenderer.enabled = true;
			} else {
				myRenderer.enabled = false;
			}
		} else {
			if (Input.GetKey (key2)) {
				myRenderer.enabled = true;
				//myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 1f);
			} else {
				myRenderer.enabled = false;
				//myRenderer.color = new Color(myRenderer.color.r, myRenderer.color.g, myRenderer.color.b, 0f);
			}
		}
	}
}
