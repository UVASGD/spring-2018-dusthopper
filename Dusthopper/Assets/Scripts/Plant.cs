using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour {

	public string myPollen;
	public GameObject food;
	[SerializeField]
	private float howFarAwayToSpawnFood;

	public void dispenseReward() {
		if (myPollen == "GreenPollen") {
			//Green plant's reward is just 2-3 food spawned in a circle around it
			Debug.Log ("green plant dispensing reward");
			int howManyFood = Random.Range (2, 4);
			Vector3 spawnPos = transform.position;
			for(int i = 0; i < howManyFood; i++){
				spawnPos += (Vector3)Random.insideUnitCircle.normalized * howFarAwayToSpawnFood;
				if ((spawnPos - GameState.asteroid.transform.position).magnitude <= GameState.asteroid.GetComponent<AsteroidInfo> ().radius) {
					GameObject newFood = GameObject.Instantiate (food, spawnPos, Quaternion.identity, this.transform.parent) as GameObject;
				} else {
					print ("attempt unsuccessful");
				}
			}
			Destroy (this.gameObject);
		}
	} 
}
