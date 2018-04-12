using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PersistSettings : MonoBehaviour {

	public Slider fXSlider;
	public Slider musicSlider;
	public AudioMixer audioMixer;
	public Slider scrollSpeedSlider;
	public PathMaker pathMaker;
	public CameraScrollOut cameraScrollOut;


	// Use this for initialization
	void OnEnable()
	{
		bool set = PlayerPrefs.GetInt("Existance", 0) == 1 ? true : false;

		if (set)
		{
			fXSlider.value = PlayerPrefs.GetFloat("FX");

			audioMixer.SetFloat("FXVol", PlayerPrefs.GetFloat("FXmix"));

			musicSlider.value = PlayerPrefs.GetFloat("MUSIC");

			audioMixer.SetFloat("musicVol", PlayerPrefs.GetFloat("Musicmix"));

			scrollSpeedSlider.value = PlayerPrefs.GetFloat("SCROLL");
			cameraScrollOut.scrollSpeed = PlayerPrefs.GetFloat("PM_scrollSpeed");
			cameraScrollOut.swapScroll = PlayerPrefs.GetInt("PM_swapScroll") ==  1 ? true : false;
			pathMaker.autoScroll = PlayerPrefs.GetInt("PM_autoScroll") ==  1 ? true : false;
		}
	}

	void OnDisable()
	{
		PlayerPrefs.SetInt("Existance", 1);

		PlayerPrefs.SetFloat("FX", fXSlider.value);
		float fxmix = 0;
		audioMixer.GetFloat("FXVol", out fxmix);
		PlayerPrefs.SetFloat("FXmix", fxmix);

		PlayerPrefs.SetFloat("MUSIC", musicSlider.value);
		float musicmix = 0;
		audioMixer.GetFloat("musicVol", out musicmix);
		PlayerPrefs.SetFloat("Musicmix", musicmix);

		PlayerPrefs.SetFloat("SCROLL", scrollSpeedSlider.value);
		PlayerPrefs.SetFloat("PM_scrollSpeed", cameraScrollOut.scrollSpeed);
		PlayerPrefs.SetInt("PM_swapScroll", cameraScrollOut.swapScroll ? 1 : 0);
		PlayerPrefs.SetInt("PM_autoScroll", pathMaker.autoScroll ? 1 : 0);
	}
}
