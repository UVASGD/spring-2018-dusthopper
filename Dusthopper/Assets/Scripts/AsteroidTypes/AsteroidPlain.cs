using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPlain : MonoBehaviour , AsteroidInterface {
    
    public AsteroidInfo info;



    public void InitDefault()
    {
        info.sensorChance = 0.35f;
        info.avgSensorRange = 30;
        info.sensorRangeRange = 15;
        info.avgSensorTimeRange = 30;
        info.sensorTimeRangeRange = 15;
        info.hasSensorColor = Color.yellow;
        info.maxItems = 5;
		info.massFactor = 1f;
        print("initdefault called");
    }

    public void Awake()
    {
        info = GetComponent<AsteroidInfo>();
    }


    public void Generate()
    {
        GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * info.maxSpeed;
        if (Random.value <= info.sensorChance)
        {
            GetComponent<AsteroidInfo>().hasSensors = true;
            GetComponent<AsteroidInfo>().sensorRange = Random.Range(info.avgSensorRange - info.sensorRangeRange, info.avgSensorRange + info.sensorRangeRange);
            GetComponent<AsteroidInfo>().sensorTimeRange = Random.Range(info.avgSensorTimeRange - info.sensorTimeRangeRange, info.avgSensorRange + info.sensorTimeRangeRange);
            GetComponent<SpriteRenderer>().color = info.hasSensorColor;
        }
        else
        {
            GetComponent<AsteroidInfo>().hasSensors = false;
        }

		info.massFactor = Mathf.Exp (Random.value * 2f) / 2f;
		gameObject.GetComponent<Rigidbody2D> ().mass = info.massFactor;

		// add property scripts
		if (Random.value <= info.chancePulledGrav) {
			gameObject.AddComponent<Gravity> ();
			info.hasGrav = true;
			//print ("Added gravity to asteroid");
		}

		if (Random.value <= info.chancePulledGrav) {
			info.pulledByGrav = true;
		}

		// spawn items
		List<ItemPoolItem> itempool = info.itempool;
        int numToSpawn = (int)(Random.value * info.maxItems);
		int numSpawned = 0;
		int randomIndex = 0;
		while (numSpawned < numToSpawn) {
			randomIndex = Random.Range (0, itempool.Count);
			float diceRoll = Random.value;
			if (diceRoll <= itempool [randomIndex].spawnChance) {
				numSpawned++;
				float distFromCenter = Random.Range (GameState.minSpawnDist, info.radius);
				Vector3 pos = Random.insideUnitCircle.normalized * distFromCenter;
				GameObject inst = GameObject.Instantiate(itempool[randomIndex].obj, transform.position + pos, Quaternion.identity, this.transform) as GameObject;
				inst.transform.parent = this.transform;
				if (itempool [randomIndex].uniqueSpawn) {
					itempool.Remove (itempool [randomIndex]);
				}
			}
		}

		//spawn decorations
		itempool = info.decorationItems;
		numToSpawn = (int)(Random.value * info.maxDecorationItems);
		numSpawned = 0;
		randomIndex = 0;
		while (numSpawned < numToSpawn) {
			randomIndex = Random.Range (0, itempool.Count);
			float diceRoll = Random.value;
			if (diceRoll <= itempool [randomIndex].spawnChance) {
				numSpawned++;
				float distFromCenter = Random.Range (GameState.minSpawnDist, info.radius);
				Vector3 pos = Random.insideUnitCircle.normalized * distFromCenter;
				GameObject inst = GameObject.Instantiate(itempool[randomIndex].obj, transform.position + pos, Quaternion.identity, this.transform) as GameObject;
				inst.transform.parent = this.transform;
				if (itempool [randomIndex].uniqueSpawn) {
					itempool.Remove (itempool [randomIndex]);
				}
			}
		}
    }
}