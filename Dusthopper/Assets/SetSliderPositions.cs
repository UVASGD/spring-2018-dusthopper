using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderPositions : MonoBehaviour {

	public Slider fXSlider;
	public Slider musicSlider;
	public Slider scrollSpeedSlider;

	void Start(){
		fXSlider.value = PlayerPrefs.GetFloat ("FX");
		musicSlider.value = PlayerPrefs.GetFloat ("MUSIC");
		scrollSpeedSlider.value = PlayerPrefs.GetFloat ("SCROLL");
	}
}
