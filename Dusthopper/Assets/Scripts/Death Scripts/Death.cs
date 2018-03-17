using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Death : MonoBehaviour {
    
    FadeController fade;
    bool isFalling = false;
    public float spinSpeed = 270.0f;

    private void Start() {
        fade = GameObject.FindObjectOfType<FadeController>();
    }

    private void Update() {

        if (isFalling) {
            //will cause the player object to spin and decrease in size, making it seem like its falling into distance
            transform.Rotate(0, 0, Time.deltaTime*spinSpeed);
            transform.localScale = new Vector3(0.991f * transform.localScale.x, 0.991f * transform.localScale.y, transform.localScale.z);

        }

    }

    /*
     * This begins the death sequence.  We start fading to black, disable a few actions, and call reloadScene()
     */
    public void Die(){

        if (GameState.isAlive == true) {
            print("begining death sequence");
            fade.fadeOut(0.33F);
            GameState.isAlive = false;
			GameState.manualJumpsDisabled = false;
            StartCoroutine(reloadScene());
        }

    }

    public IEnumerator reloadScene() {

        yield return new WaitForSeconds(2.0F);

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        GameState.isAlive = true;

    }

    public void delayDie(float time) {

        isFalling = true;
        Invoke("Die", time);

    }

}
