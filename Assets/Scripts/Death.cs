using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Death : MonoBehaviour {

    FadeController fade;

    private void Start() {
        fade = GameObject.FindObjectOfType<FadeController>();
    }

    public void Die(){
		print ("begining death sequence");
	}

    //Damon Work

    public bool runOnce = false;

    private void Update() {

        if (runOnce == false) {
            if (Time.fixedTime > 5) {
                runOnce = true;
                print("calling death sequence");
                fade.fadeIn(1.0F);
            } else {
                print("Time.fixedTime: " + Time.fixedTime);
            }
        }

    }

    public void reloadScene() {

        print("reloading scene");
        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.name);
    }
}
