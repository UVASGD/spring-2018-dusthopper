using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualJump : MonoBehaviour
{
    //Allows the player to charge up a jump by holding down left mouse button and then jumps to the first asteroid in the direction of cursor.
    //TODO: sound effect weirdness
    private float timeHeld = 0f;
    public float timeCancelSchedule = 0.75f;
    private bool hasCanceled = false;
    public AudioSource jump;
    public GameObject gameManager;
	public bool manuallyJumping;

    bool playingSoundFromHere = false;

    void Start()
    {
        timeHeld = 0f;
		manuallyJumping = false;
    }
    // Update is called once per frame
    void Update()
    {
        //		print (timeHeld);
        if (!GameState.mapOpen && !GameState.gamePaused)
        {
            if (Input.GetMouseButton(0))
            {
                if (timeHeld >= GameState.secondsPerJump)
                {
                    Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 directionOfCursor = (Vector2)(cursorPosition - transform.position);
                    int onlyAsteroids = (1 << LayerMask.NameToLayer("Asteroid"));
                    RaycastHit2D[] thingsIHit = Physics2D.RaycastAll((Vector2)transform.position, directionOfCursor, GameState.maxAsteroidDistance, onlyAsteroids);
                    if (thingsIHit.Length > 1)
                    {
                        Transform otherAsteroid = thingsIHit[1].transform; // thingsIHit[0]  is the asteroid we're standing on so we want the next one
                                                                           //						print(otherAsteroid.gameObject.name);
                        GetComponent<Movement>().SwitchAsteroid(otherAsteroid);
                    }
                    else
                    {
                        print("didn't hit anything");
                        //"jump" to point in space at end of raycast and die / lose a life
                        JumpFail(directionOfCursor.normalized * GameState.maxAsteroidDistance + (Vector2)transform.position);
                    }
                    timeHeld = 0;
                    hasCanceled = false;
					manuallyJumping = false;
                    jump.Stop();
                }
                else
                {
                    if (timeHeld >= timeCancelSchedule)
                    {
                        GameState.manualJumpsDisabled = false;
                        if (!hasCanceled)
                        {

                            gameManager.GetComponent<PathMaker>().RemoveJumps();
                            hasCanceled = true;
                        }
                    }
                    timeHeld += Time.deltaTime;
					manuallyJumping = true;
                    if (!jump.isPlaying)
                    {
                        playingSoundFromHere = true;
                        jump.Play();
                    }
                }
            }
            else
            {
                timeHeld = 0;
                hasCanceled = false;
                if (jump.isPlaying && playingSoundFromHere)
                {
					manuallyJumping = false;
                    jump.Stop();
                    playingSoundFromHere = false;
                }
            }
        }
    }

    // If you click and hold and there's no asteroid in your path
    void JumpFail(Vector3 targPos)
    {
        //print ("Jump fail! Going to point " + targPos);
        //GameState.asteroid = null;
        GameState.manualJumpsDisabled = true;
        //		GameState.player.transform.parent = null;
        GameObject target = new GameObject("FailJumpPoint");
        target.transform.position = targPos;
        GetComponent<Movement>().SwitchAsteroid(target.transform, false);
        //print ("WHAT");

        GetComponent<Death>().delayDie(0.75f);
    }

}