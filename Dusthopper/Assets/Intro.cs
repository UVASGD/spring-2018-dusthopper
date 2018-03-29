using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FadeController), typeof(AudioSource))]
public class Intro : MonoBehaviour {

	FadeController fade;
	private bool buttonPressed;
	private AudioSource buttonPressedAudio;

	// Use this for initialization
	void Awake () {
		fade = GetComponent<FadeController> ();
		buttonPressedAudio = GetComponent<AudioSource> ();
	}

	void Update () {
		if (Input.anyKeyDown) {
			fade.fadeOut(0.4f);
			buttonPressed = true;
			buttonPressedAudio.Play ();
		}

		if (fade.anim.GetCurrentAnimatorStateInfo(0).IsName("FadedOut") && buttonPressed) {
			StartGame ();
		}
	}
	
	void StartGame () {
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
	}
}
