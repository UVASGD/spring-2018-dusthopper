using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

	private GameObject hub;
	private Camera cam;

	private Vector3 camVel;
	private float camScale;

	private float rotateSpeed = 0f;
	private float refRotate;
	private bool canRotate;

	private AudioSource endAudio;
	private float audioVel;

	// Use this for initialization
	void Awake () {
		GameState.endGame = false;
		hub = GameObject.FindGameObjectWithTag ("Hub");
		cam = Camera.main;
		rotateSpeed = 0f;
		canRotate = false;
		endAudio = transform.Find ("SFX").Find ("GravitySFX").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.O) && !GameState.endGame) {
			End ();
		}

		if (GameState.endGame) {
			cam.transform.position = Vector3.SmoothDamp (cam.transform.position, Vector3.back * 10, ref camVel, 5f);
			cam.orthographicSize = Mathf.SmoothDamp (cam.orthographicSize, 20, ref camScale, 20f);

			if (canRotate) {
				hub.transform.Find ("GravPoints").Rotate (Vector3.forward * -rotateSpeed * Time.deltaTime);
				rotateSpeed = Mathf.SmoothDamp (rotateSpeed, 120f, ref refRotate, 10f);
			}

			if (endAudio.volume < 1) {
				endAudio.volume += Time.unscaledDeltaTime * 0.1f;
			}

			if (endAudio.pitch < 3) {
				endAudio.pitch = Mathf.SmoothDamp (endAudio.pitch, 3, ref audioVel, 20f);
			}
		}
	}

	public void CanRotate (){
		canRotate = true;
	}

	public void EndIfAble () {
		Invoke ("CanRotate", 10f);
		if (GameState.gravityFragmentCount >= 3) {
			End ();
		}
	}

	public void End () {
		GameObject[] asteroids = GameObject.FindGameObjectsWithTag ("Asteroid");

		foreach (GameObject asteroid in asteroids) {
			asteroid.AddComponent<ConvergeOnHub> ();
			//asteroid.GetComponent<Collider2D> ().enabled = false;
		}
		GameState.endGame = true;
		cam.transform.SetParent (null);
		cam.GetComponent<SmoothCamera2D> ().target = null;
		hub.transform.position = Vector3.zero;
		hub.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		hub.GetComponent<Rigidbody2D> ().isKinematic = true;
		hub.AddComponent<ShakeObject> ();
		endAudio.Play ();

		Invoke ("CallFade", 30f);
		Invoke ("Credits", 40f);

		GameState.gravityFragmentCount = 0;
	}

	private void CallFade () {
		FindObjectOfType<FadeController> ().fadeOut (0.1f);
	}

	private void Credits () {
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Credits");
	}
}
