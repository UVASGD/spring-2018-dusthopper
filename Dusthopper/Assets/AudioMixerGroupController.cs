using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class AudioMixerGroupController : MonoBehaviour {

	public AnimationCurve animCurve;
	public AudioMixer masterMix;
	public string paramName;
	private Slider slider;
	private float shadowValue;

	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider>();
		slider.value = 0.5f;
		shadowValue = slider.value;
	}
	
	// Update is called once per frame
	void Update () {
		if(shadowValue != slider.value)
		{
			shadowValue = slider.value;
			float f = animCurve.Evaluate(slider.value);
			bool success = masterMix.SetFloat(paramName, f);
		}
	}
}
