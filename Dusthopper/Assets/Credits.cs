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
				if (Input.anyKeyDown) {
					Invoke ("FadeOut", 0f);
					Invoke ("BackToMainMenu", 5f);
				}

				Invoke ("FadeOut", 125f);
				Invoke ("BackToMainMenu", 130f);
			}
		}

		if (hasEnded) {
		GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>().volume -= Time.deltaTime * 0.2f;
		}
	}

	void FadeOut () {
		if (!hasEnded) {
			FindObjectOfType<FadeController> ().fadeOut (0.2f);
			hasEnded = true;
		}
	}

	void BackToMainMenu () {
		SceneManager.LoadScene ("Logo");
	}
}
