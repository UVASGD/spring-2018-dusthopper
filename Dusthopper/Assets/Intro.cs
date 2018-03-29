using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour {

	State myState;
	FadeController fade;

	// Use this for initialization
	void Start () {
		fade = GetComponent<FadeController> ();
	}

	void Update () {
		if (Input.anyKeyDown) {
			fade.fadeOut(0.4f);
			Invoke ("StartGame", 3f);
		}

//		switch (myState) {
//		case State.fadeIn:
//			
//			break;
//
//		default:
//		case State.full:
//			if (Input.anyKeyDown) {
//				fade.fadeOut(1f);
//				Invoke ("StartGame", 3f);
//			}
//			break;
//
//		case State.fadeOut:
//
//			break;
//		}
	}
	
	void StartGame () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
	}

	void SetState () {

	}

	public enum State { fadeIn, full, fadeOut };
}
