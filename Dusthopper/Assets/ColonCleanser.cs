using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonCleanser : MonoBehaviour {

    public GameObject important;
    public Plant Be_enis;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (important != null && Input.GetButtonDown("Jump")) {
            Be_enis.Bloom(important);
            Destroy(important);
        }
	}
}
