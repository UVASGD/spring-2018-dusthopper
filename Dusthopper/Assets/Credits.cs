using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

	public float scrollSpeed = 0.2f;

	// Use this for initialization
	void Start () {
		Invoke ("FadeOut", 30f);
		Invoke ("BackToMainMenu", 35f);
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < 1350) {
			transform.position += Vector3.up * Time.deltaTime * scrollSpeed;
		} else {
			//transform.position = new Vector3 (transform.position.x, 1350, transform.position.z);
		}
	}

	void FadeOut () {
		FindObjectOfType<FadeController> ().fadeOut (0.2f);
	}

	void BackToMainMenu () {
		SceneManager.LoadScene ("Logo");
	}
}
