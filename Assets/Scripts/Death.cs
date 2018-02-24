using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Death : MonoBehaviour {

    public bool runOnce = true;
    FadeController fade;

    private void Start() {
        fade = GameObject.FindObjectOfType<FadeController>();
    }

    public void Die(){
		print ("begining death sequence");
        fade.fadeOut(1.0F);
        StartCoroutine(reloadScene());
    }

    //Damon Work

    private void Update() {

        if (runOnce == true) {
            if (Time.fixedTime > 2) {
                print("runOnce: " + runOnce);
                runOnce = false;
                Die();
            } else {
                print("Time.fixedTime: " + Time.fixedTime);
            }
        
        }

    }

    public IEnumerator reloadScene() {

        print("starting to wait 1 second before reloading scene");
        yield return new WaitForSeconds(1.0F);

        print("reloading scene");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        print("reload scene done, fading in");
        fade.fadeIn(1.0F);

    }
}
