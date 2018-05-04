using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	public string myPollen;
	public GameObject food;
	[SerializeField]
	private float howFarAwayToSpawnFood;
	public GameObject scrap;
	[SerializeField]
	private float howFarAwayToSpawnScrap;
    [HideInInspector]
    public bool bloomed;

	public float blueTime;


    public void Bloom(GameObject pollen) {
        Transform mo = transform.GetChild(0);
        mo.position = pollen.transform.position;

        GetComponent<Animator>().SetTrigger("Open");
        bloomed = true;
    }

    void KillPollen() {
        Destroy(transform.Find("Pollen").gameObject);
    }

    void DispenseReward() {
        KillPollen();

        GameObject firstFood = GameObject.Instantiate (food, this.transform.position, Quaternion.identity, this.transform.parent) as GameObject; //all plants should spawn 1 food
		if (myPollen == "GreenPollen") {
			//Green plant's reward is just 1 or 2 additional food spawned in a circle around it
//			Debug.Log ("green plant dispensing reward");
			int howManyFood = Random.Range (1, 3);
			Vector3 spawnPos = transform.position;
			for(int i = 0; i < howManyFood; i++){
				spawnPos += (Vector3)Random.insideUnitCircle.normalized * howFarAwayToSpawnFood;
				if ((spawnPos - GameState.asteroid.transform.position).magnitude <= GameState.asteroid.GetComponent<AsteroidInfo> ().radius) {
					GameObject newFood = GameObject.Instantiate (food, spawnPos, Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward), this.transform.parent) as GameObject;
				} else {
//					print ("attempt unsuccessful");
				}
			}
			this.transform.parent.GetComponent<AsteroidInfo> ().greenPlantCount -= 1;
		}
		if (myPollen == "YellowPollen") {
			//Yellow plant's reward is to spawn 3 to 5 scrap 
			Debug.Log ("yellow plant dispensing reward");
			int howManyScrap = Random.Range (3, 6);
			Vector3 spawnPos = transform.position;
			for(int i = 0; i < howManyScrap; i++){
				spawnPos += (Vector3)Random.insideUnitCircle.normalized * howFarAwayToSpawnScrap;
				if ((spawnPos - GameState.asteroid.transform.position).magnitude <= GameState.asteroid.GetComponent<AsteroidInfo> ().radius) {
					GameObject newScrap = GameObject.Instantiate (scrap, spawnPos, Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward), this.transform.parent) as GameObject;
				} else {
					print ("attempt unsuccessful");
				}
			}
			this.transform.parent.GetComponent<AsteroidInfo> ().yellowPlantCount -= 1;
		}
		if (myPollen == "RedPollen") {
			Debug.Log ("red plant dispensing reward");
            // Give player a super jump, then open map
            GameState.maxAsteroidDistance = GameState.grayDistFactor*GameState.maxAsteroidDistance;
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<PathMaker> ().RemoveJumps ();
			Camera.main.GetComponent<CameraScrollOut> ().justGotGrayPollen = true;
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<PathMaker> ().specialGrayPollenJump = true;
			Camera.main.GetComponent<CameraScrollOut>().openMap ();
			this.transform.parent.GetComponent<AsteroidInfo> ().redPlantCount -= 1;
		}
		if (myPollen == "BluePollen") {
			Debug.Log ("blue plant dispensing reward");
			// Give player less charging time
			GameState.player.GetComponent<PlayerCollision>().setBlueTimer(blueTime);
			this.transform.parent.GetComponent<AsteroidInfo> ().bluePlantCount -= 1;
		}
	} 


}
