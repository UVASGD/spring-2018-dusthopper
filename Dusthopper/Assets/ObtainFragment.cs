using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainFragment : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			GameObject.Find("GM").transform.Find("SFX").Find("ObjectiveSFX").GetComponent<AudioSource>().Play();
			GameObject.Find("GM").transform.Find("SFX").Find("Music").GetComponent<AudioSource>().PlayDelayed(10f);
			Destroy (gameObject);
		}
	}
}
