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
		//TODO: make something actually happen when you die.
<<<<<<< HEAD
		print ("begining death sequence");
=======
//		print ("ripperoni");
>>>>>>> 3db511585a8e7c079dc979385aa23542e381ec22
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
