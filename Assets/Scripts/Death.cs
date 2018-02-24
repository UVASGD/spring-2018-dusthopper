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
        if (GameState.isAlive == true) {
            print("begining death sequence");
            float speed = 1.0F;     //the speed at which the screen fades to black     

            GameState.isAlive = false;
            StartCoroutine(reloadScene(speed));
        }
    }

    public IEnumerator reloadScene(float speed) {

        //It will take (speed) time to fade to black.  We'll start fading and then pause until thats finished.
        fade.fadeOut(speed);
        yield return new WaitForSeconds(speed);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        GameState.isAlive = true;
        print("reload scene done");

    }
}
