using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecayUnscaled : MonoBehaviour {
    public float RComp;
    public float GComp;
    public float BComp;
    public float decayTimer;
    public bool decay = false;
    
    /*
     * This class will slowly decay a text object that it's attached to.
     */

    //record initial colors
	void Start () {
        RComp = GetComponent<Text>().color.r;
        GComp = GetComponent<Text>().color.g;
        BComp = GetComponent<Text>().color.b;
    }
	
    //Will slowly decay the text object until its completely transparent (alpha = 0)
	void OnGUI () {

        if (decay) {

            float currentAlpha = GetComponent<Text>().color.a;
            GetComponent<Text>().color = new Color(RComp, GComp, BComp, currentAlpha - decayTimer * Time.unscaledDeltaTime);

            if (currentAlpha == 0) {
                decay = false;
            }
        }

	}

    //This is the primary method.  Is called from outside to initially set non-transparent, then will slowly decay back to transparent
    public void setOpaque() {
        GetComponent<Text>().color = new Color(RComp, GComp, BComp, 1f);
        decay = true;
    }



}
