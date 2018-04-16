using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderPositions : MonoBehaviour {

	public Slider fXSlider;
	public Slider musicSlider;
	public Slider scrollSpeedSlider;
//	public Toggle autoScrollToggle;
//	public Toggle swapScrollToggle;

	void Start(){
		fXSlider.value = PlayerPrefs.GetFloat ("FX");
		musicSlider.value = PlayerPrefs.GetFloat ("MUSIC");
		scrollSpeedSlider.value = PlayerPrefs.GetFloat ("SCROLL");
//		swapScrollToggle.isOn = PlayerPrefs.GetInt("PM_swapScroll") ==  1 ? true : false;
//		autoScrollToggle.isOn = PlayerPrefs.GetInt("PM_autoScroll") ==  1 ? true : false;
	}
}
