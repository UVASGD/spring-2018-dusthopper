using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Death : MonoBehaviour {
    
    FadeController fade;
    bool isFalling = false;
    public float spinSpeed = 270.0f;

	public AudioSource fallIntoSpace;

    private void Start() {
        fade = GameObject.FindObjectOfType<FadeController>();
    }

    private void Update() {

        //this is used by class ManualJump.  When isFalling is set to true the player object will start spinning and shrinking, to represent dying by falling into the void
        if (isFalling) {
            transform.Rotate(0, 0, Time.deltaTime*spinSpeed);
            transform.localScale = new Vector3(0.987f * transform.localScale.x, 0.987f * transform.localScale.y, transform.localScale.z);

        }

    }

    /*
     * Will cause the scene to start fading to black, and then call the helper method reloadScene()
     */
    public void Die(){
		if (GameState.tutorialCompleted) {
			if (GameState.isAlive == true) {
				fade.fadeOut (0.45f);    //slightly slower rate than just fading to black by the time scene is reloaded
				GameState.isAlive = false;
				GameState.manualJumpsDisabled = false;
				StartCoroutine (reloadScene ());
			}
		} else {
			GameState.ResetGame ();
			isFalling = false;
			//GameState.player.transform.position = GameObject.FindWithTag ("Hub").transform.position;
			GameState.player.transform.localScale = Vector3.one * 0.04f;
		}
    }

	public void PlayFallingSound(){
		fallIntoSpace.Play ();
	}
    
    /*
     * Will reload the scene after a slight delay
     * This is a helper method of Die()
     */
    public IEnumerator reloadScene() {

        yield return new WaitForSeconds(2.0f);  

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        GameState.isAlive = true;
        GameState.time = 0;     //fixes a bug where some systems (such as .canEat() in food) relies on game start time = 0

    }

    /*
     * This method is mainly used by class ManualJump.  It will begin the spin-shrink die system in update() that we want when you fall into the void, and delay the beggining of the fade to black
     */
    public void delayDie(float time) {

        isFalling = true;   //allows actions in update() to run
        Invoke("Die", time);  //call method Die() after var. time seconds
		Invoke("PlayFallingSound", 0.6f);
    }

}
