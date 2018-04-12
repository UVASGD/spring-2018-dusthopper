using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicAudioController : MonoBehaviour {

	private Slider volumeSlider;
	private AudioSource audio;

	// Use this for initialization
	void Start () {
		volumeSlider = GameObject.Find ("Canvas").transform.Find ("SettingsMenu").Find ("MusicVolumeSlider").GetComponent<Slider> ();
		audio = GetComponent<AudioSource> ();
	}

	void Update () {
		audio.volume = volumeSlider.value;
	}


	public void OnValueChanged () {

	}
}
