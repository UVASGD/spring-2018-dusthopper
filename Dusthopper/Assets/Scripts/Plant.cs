using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	public string myPollen;
	public GameObject food;
	[SerializeField]
	private float howFarAwayToSpawnFood;

    public void dispenseReward() {
		GameObject firstFood = GameObject.Instantiate (food, this.transform.position, Quaternion.identity, this.transform.parent) as GameObject; //all plants should spawn 1 food
		if (myPollen == "GreenPollen") {
			//Green plant's reward is just 1 or 2 additional food spawned in a circle around it
			Debug.Log ("green plant dispensing reward");
			int howManyFood = Random.Range (1, 3);
			Vector3 spawnPos = transform.position;
			for(int i = 0; i < howManyFood; i++){
				spawnPos += (Vector3)Random.insideUnitCircle.normalized * howFarAwayToSpawnFood;
				if ((spawnPos - GameState.asteroid.transform.position).magnitude <= GameState.asteroid.GetComponent<AsteroidInfo> ().radius) {
					GameObject newFood = GameObject.Instantiate (food, spawnPos, Quaternion.identity, this.transform.parent) as GameObject;
				} else {
					print ("attempt unsuccessful");
				}
			}
		}
		Destroy (this.gameObject); //all plants should destroy themselves
		//TODO: instead of destroying self, set Physics2D.ignoreCollision or whatever it is and start "bloom" animation
	} 


}
