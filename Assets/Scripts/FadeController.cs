using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FadeController : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void fadeIn(float speed)
    {
        anim.speed = speed;
        anim.SetTrigger("FadeIn");
    }

    public void fadeOut(float speed)
    {
        anim.speed = speed;
        anim.SetTrigger("FadeOut");

    }
}
