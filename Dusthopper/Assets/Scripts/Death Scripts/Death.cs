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

        //this is used by class ManualJump.  When isFalling is set to true the player object will start spinning and shrinking, to represent dying by falling into the void
        if (isFalling) {
            transform.Rotate(0, 0, Time.deltaTime*spinSpeed);
            transform.localScale = new Vector3(0.991f * transform.localScale.x, 0.991f * transform.localScale.y, transform.localScale.z);

        }

    }

    /*
     * Will cause the scene to start fading to black, and then call the helper method reloadScene()
     */
    public void Die(){

        if (GameState.isAlive == true) {
            fade.fadeOut(0.33F);    //0.33f = fade over 3 seconds
            GameState.isAlive = false;
			GameState.manualJumpsDisabled = false;
            StartCoroutine(reloadScene());
        }

    }
    
    /*
     * Will reload the scene after a slight delay
     * This is a helper method of Die()
     */
    public IEnumerator reloadScene() {

        yield return new WaitForSeconds(2.0F);  

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        GameState.isAlive = true;

    }

    /*
     * This method is mainly used by class ManualJump.  It will begin the spin-shrink die system in update() that we want when you fall into the void, and delay the beggining of the fade to black
     */
    public void delayDie(float time) {

        isFalling = true;   //allows actions in update() to run
        Invoke("Die", time);  //call method Die() after var. time seconds

    }

}
