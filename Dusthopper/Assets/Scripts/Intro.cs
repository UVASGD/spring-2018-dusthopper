using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(FadeController), typeof(AudioSource))]
public class Intro : MonoBehaviour {

	//For End of Semester Expo. If true, will always load tutorial regardless of whether or not it is completed
	public bool forceTutorial = false;

	FadeController fade;
	private bool buttonPressed;
	private AudioSource buttonPressedAudio;
	public Text continueText;

	// Use this for initialization
	void Awake () {
		buttonPressed = false;
		fade = GetComponent<FadeController> ();
		buttonPressedAudio = GetComponent<AudioSource> ();
		continueText.color = new Color (continueText.color.r, continueText.color.g, continueText.color.b, 0f);
		StartCoroutine ("ShowButton");
		GameState.LoadGame ();
	}

	void Update () {
//		print ("Tutorial Completed: " + GameState.tutorialCompleted);
		if (Input.anyKeyDown && fade.anim.GetCurrentAnimatorStateInfo(0).IsName("Faded") && ! buttonPressed) {
			fade.fadeOut(0.4f);
			buttonPressed = true;
			buttonPressedAudio.Play ();
		}

		if (fade.anim.GetCurrentAnimatorStateInfo(0).IsName("FadedOut") && buttonPressed) {
			StartGame ();
		}
	}
	
	void StartGame () {
		
		if (GameState.tutorialCompleted && !forceTutorial) {
			SceneManager.LoadScene ("MainGame");
		} else { 
			SceneManager.LoadScene ("Tutorial");
		}
	}

	IEnumerator ShowButton () {
		yield return new WaitForSeconds (3f);
		while (continueText.color.a < 1) {
			continueText.color += new Color(0, 0, 0, 1 * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}
	}
}
