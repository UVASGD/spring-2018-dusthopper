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
            fade.fadeOut(1.0F);
            GameState.isAlive = false;
            StartCoroutine(reloadScene());
        }
    }

    public IEnumerator reloadScene() {

        yield return new WaitForSeconds(1.0F);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        GameState.isAlive = true;
        print("reload scene done");

    }
}
