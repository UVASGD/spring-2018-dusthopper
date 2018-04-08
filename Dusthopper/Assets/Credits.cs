using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

	public float scrollSpeed = 0.2f;
	public bool hasEnded;

	// Use this for initialization
	void Start () {
		hasEnded = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < Screen.height / 2 + 1400) {
			transform.position += Vector3.up * Time.deltaTime * scrollSpeed;
		} else {
			if (!hasEnded) {
				Invoke ("FadeOut", 5f);
				Invoke ("BackToMainMenu", 10f);
				hasEnded = true;
			}
		}
	}

	void FadeOut () {
		FindObjectOfType<FadeController> ().fadeOut (0.2f);
	}

	void BackToMainMenu () {
		SceneManager.LoadScene ("Logo");
	}
}
