using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecayUnscaled : MonoBehaviour {
    public float RComp;
    public float GComp;
    public float BComp;
    public float decayTimer;

	// Use this for initialization
	void Start () {
        RComp = GetComponent<Text>().color.r;
        GComp = GetComponent<Text>().color.g;
        BComp = GetComponent<Text>().color.b;
    }
	
	// Update is called once per frame
	void OnGUI () {

        float currentAlpha = GetComponent<Text>().color.a;
        GetComponent<Text>().color = new Color(RComp, GComp, BComp, currentAlpha - decayTimer * Time.unscaledDeltaTime);


	}

    public void setOpaque() {

        print("ran setOpaque");
        GetComponent<Text>().color = new Color(RComp, GComp, BComp, 1f);
    }



}
