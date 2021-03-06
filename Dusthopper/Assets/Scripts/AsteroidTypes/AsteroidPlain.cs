﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPlain : MonoBehaviour, AsteroidInterface {

    public AsteroidInfo info;



    public void InitDefault() {
        info.goalArrowsVisibleChance = 0.75f;
        info.sensorChance = 0.35f;
        info.avgSensorRange = 30;
        info.sensorRangeRange = 15;
        info.avgSensorTimeRange = 30;
        info.sensorTimeRangeRange = 15;
        info.maxItems = 5;
        info.massFactor = 1f;
        print("initdefault called");
    }

    public void OnEnable() {
        info = GetComponent<AsteroidInfo>();
    }


    public void Generate() {
        //Debug.Log(info);
        //Debug.Log(GetComponent<Rigidbody2D>());
        GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * info.maxSpeed;
        if (Random.value <= info.sensorChance) {
            GetComponent<AsteroidInfo>().hasSensors = true;
            GetComponent<AsteroidInfo>().sensorRange = Random.Range(info.avgSensorRange - info.sensorRangeRange, info.avgSensorRange + info.sensorRangeRange);
            GetComponent<AsteroidInfo>().sensorTimeRange = Random.Range(info.avgSensorTimeRange - info.sensorTimeRangeRange, info.avgSensorRange + info.sensorTimeRangeRange);
            if (Random.value <= info.goalArrowsVisibleChance) {
                GetComponent<AsteroidInfo>().goalArrowsVisible = true;
            }
        } else {
            GetComponent<AsteroidInfo>().hasSensors = false;

        }

        info.massFactor = Mathf.Exp(Random.value * 2f) / 2f;
        gameObject.GetComponent<Rigidbody2D>().mass = info.massFactor;

        // add property scripts
        if (Random.value <= info.chancePulledGrav) {
            gameObject.AddComponent<Gravity>();
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
            randomIndex = Random.Range(0, itempool.Count);
            float diceRoll = Random.value;
            if (diceRoll <= itempool[randomIndex].spawnChance) {
                numSpawned++;
                float distFromCenter = Random.Range(GameState.minSpawnDist, info.radius);
                Vector3 pos = Random.insideUnitCircle.normalized * distFromCenter;
                GameObject inst = GameObject.Instantiate(itempool[randomIndex].obj, transform.position + pos, Quaternion.identity, this.transform) as GameObject;
                inst.transform.parent = this.transform;
//				print (inst.name);
                if (inst.tag == "Plant") {
                    if (inst.name.ToLower().Contains("green")) {
                        this.GetComponent<AsteroidInfo>().greenPlantCount += 1;
                    }
                    if (inst.name.ToLower().Contains("blue")) {
                        this.GetComponent<AsteroidInfo>().bluePlantCount += 1;
                    }
                    if (inst.name.ToLower().Contains("yellow")) {
                        this.GetComponent<AsteroidInfo>().yellowPlantCount += 1;
                    }
                    if (inst.name.ToLower().Contains("red")) {
                        this.GetComponent<AsteroidInfo>().redPlantCount += 1;
                    }
                }
                if (itempool[randomIndex].uniqueSpawn) {
                    itempool.Remove(itempool[randomIndex]);
                }
            }
        }

        //spawn decorations
        itempool = info.decorationItems;
        numToSpawn = (int)(Random.value * info.maxDecorationItems);
        numSpawned = 0;
        randomIndex = 0;
        while (numSpawned < numToSpawn) {
            randomIndex = Random.Range(0, itempool.Count);
            float diceRoll = Random.value;
            if (diceRoll <= itempool[randomIndex].spawnChance) {
                numSpawned++;
                float distFromCenter = Random.Range(GameState.minSpawnDist, info.radius);
                Vector3 pos = Random.insideUnitCircle.normalized * distFromCenter;
                GameObject inst = GameObject.Instantiate(itempool[randomIndex].obj, transform.position + pos, Quaternion.identity, this.transform) as GameObject;
                inst.transform.parent = this.transform;
                if (itempool[randomIndex].uniqueSpawn) {
                    itempool.Remove(itempool[randomIndex]);
                }
            }
        }
    }
}