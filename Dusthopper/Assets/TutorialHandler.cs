using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour {
    int tutorialStage = 0;
    bool conditionMet = false;
    Requirement currentRequirement;
    float timeSinceLastStage = 0f;
    float timeSpentMoving = 0f;

    bool nextStage;

    bool canMove;
    bool canZoom;
    bool canJump;
    bool canPlanJump;

    private Movement playerMove;
    private CameraScrollOut camScroll;
    private ManualJump playerJump;
    private PathMaker pathMaker;
    private GameObject plant;

    // icons mah boi
    public GameObject KeysButton, LeftMouseButton, RightMouseButton, ScrollButton;

    //private Transform hub;

    // Use this for initialization
    void Start() {
        tutorialStage = 0;
        GameState.tutorialCompleted = false;
        conditionMet = false;
        timeSinceLastStage = 0f;
        timeSpentMoving = 0f;
        nextStage = false;
        canMove = false;
        canZoom = false;
        canJump = false;
        canPlanJump = false;
        GameState.hunger *= 0.9f;

        currentRequirement = Requirement.none;

        playerMove = GameState.player.GetComponent<Movement>();
        camScroll = Camera.main.GetComponent<CameraScrollOut>();
        playerJump = GameState.player.GetComponent<ManualJump>();
        pathMaker = FindObjectOfType<PathMaker>();
        plant = FindObjectOfType<Plant>().gameObject;

        //hub = GameObject.FindWithTag ("Hub");

        int index = 0;
        foreach (Transform child in transform) {
            if (index++ > 0) {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void ButtonRequirement() {
        nextStage = true;
    }

    // Update is called once per frame
    void Update() {

        if (GameState.asteroid.tag != "Hub" && GameState.asteroid.tag != "Asteroid") {
            //print ("RESET");
            GameState.ResetGame();
        }

        conditionMet = false;
        switch (currentRequirement) {
            default:
                Debug.LogError("Requirement Not Valid!");
                break;
            case Requirement.none:
                conditionMet = nextStage;
                break;
            case Requirement.move:
                if (KeysButton != null) KeysButton.SetActive(true);
                //	canMove = true;
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
                    timeSpentMoving += Time.unscaledDeltaTime;
                }

                if (timeSpentMoving >= 2f) {
                    conditionMet = true;
                    if(KeysButton!=null) KeysButton.GetComponent<Animator>().SetTrigger("Kill");
                }
                break;
            case Requirement.manualJump:
                if (LeftMouseButton != null) LeftMouseButton.SetActive(true);
                if (GameState.asteroid.tag != "Hub") {
                    conditionMet = true;
                    if (LeftMouseButton != null) LeftMouseButton.GetComponent<Animator>().SetTrigger("Kill");
                }
                break;
            case Requirement.openMap:
                if (GameState.mapOpen) {
                    if (ScrollButton != null) ScrollButton.SetActive(true);
                    conditionMet = true;
                    if (ScrollButton != null) ScrollButton.GetComponent<Animator>().SetTrigger("Kill");
                }
                break;
            case Requirement.planJump:
                if (pathMaker.path.Count > 0) {
                    conditionMet = true;
                }
                break;
            case Requirement.closeMap:
                if (!GameState.mapOpen) {
                    conditionMet = true;
                }
                break;
            case Requirement.eatFood:
                if (GameState.hunger == GameState.maxHunger && timeSinceLastStage >= 1f) {
                    conditionMet = true;
                }
                break;
            case Requirement.collectScrap:

                break;
            case Requirement.pickupSeed:
                if (GameState.player.transform.childCount > 1) {
                    conditionMet = true;
                }
                break;
            case Requirement.depositSeed:
                if (plant == null) {
                    conditionMet = true;
                }
                break;
            case Requirement.returnToHub:
                if (GameState.asteroid.tag == "Hub") {
                    conditionMet = true;
                }
                break;
        }

        timeSinceLastStage += Time.unscaledDeltaTime;

        playerMove.canMove = canMove;
        camScroll.enabled = canZoom;
        playerJump.enabled = canJump;
        pathMaker.tutorialAllows = canPlanJump;


        if (conditionMet) {
            NextStage();
        }
    }

    void NextStage() {
        Transform currentChild;
        Transform nextChild;

        if (tutorialStage < transform.childCount) {
            currentChild = transform.GetChild(tutorialStage++);
        } else {
            EndTutorial();
            return;
        }

        if (tutorialStage < transform.childCount) {
            nextChild = transform.GetChild(tutorialStage);
        } else {
            EndTutorial();
            return;
        }

        currentChild.gameObject.SetActive(false);

        TutorialTextBox next = nextChild.GetComponent<TutorialTextBox>();

        nextChild.gameObject.SetActive(true);
        currentRequirement = next.requirement;

        nextStage = false;

        canMove = next.canMove;
        canZoom = next.canZoom;
        canJump = next.canJump;
        canPlanJump = next.canPlanJump;

        timeSinceLastStage = 0f;
    }

    public void EndTutorial() {
        GameState.ResetGame();
        GameState.tutorialCompleted = true;
        GameState.SaveGame();

        Invoke("FadeOut", 0f);
        Invoke("StartGame", 5f);
    }

    void FadeOut() {
        FindObjectOfType<FadeController>().fadeOut(0.2f);
    }

    void StartGame() {
        SceneManager.LoadScene("MainGame");
    }
}
