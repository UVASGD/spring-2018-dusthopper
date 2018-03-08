using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finished for now
public class ApplicationManager : MonoBehaviour {
	//handles quitting and target framerate

	[SerializeField] [Range(0, 4)] private int vSyncCount = 1;
	private int vSyncCountCached = 1;

	[SerializeField] [Range(30, 120)] private int targetFrameRate = 60;
	private int targetFrameRateCached = 60;

	private const float timeToWait = 2.0f;
	private float timer;

	// Use this for initialization
	void Start () {
		timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Q)) {
			QuitGame ();
		}

		if (this.targetFrameRateCached != this.targetFrameRate || vSyncCountCached != vSyncCount) 
		{
			timer += Time.unscaledDeltaTime;
			if (timer > timeToWait) {
				ChangeTFR ();
			}
		}
	}

	private void ChangeTFR()
	{
		QualitySettings.vSyncCount = this.vSyncCount;
		this.vSyncCountCached = this.vSyncCount;
		Application.targetFrameRate = this.targetFrameRate;
		this.targetFrameRateCached = this.targetFrameRate;
		timer = 0;
		if (QualitySettings.vSyncCount > 0) {
			Debug.Log ("Ignoring change to Target Framerate becasue QualitySettings.vSyncCount is set.");
		} else {
			Debug.Log ("Changing Target Framerate to " + this.targetFrameRate/this.vSyncCount);
		}
	}

	void QuitGame () {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
		#if UNITY_STANDALONE
			Application.Quit();
		#endif
	}
}
