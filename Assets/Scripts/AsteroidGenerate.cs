﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidGenerate : MonoBehaviour {

    //proc gen sensor stuff
    public Color hasSensorColor;
    public float sensorChance;
    public int avgSensorRange;
    public int sensorRangeRange;
    public int avgSensorTimeRange;
    public int sensorTimeRangeRange;

    public float maxSpeed = 5; //how fast can they be going at the start?

    public void generate(int asteroidNum){
        name = "Asteroid" + asteroidNum.ToString();
        GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * maxSpeed;
        if (Random.value <= sensorChance)
        {
            GetComponent<AsteroidSensorInfo>().hasSensors = true;
            GetComponent<AsteroidSensorInfo>().sensorRange = Random.Range(avgSensorRange - sensorRangeRange, avgSensorRange + sensorRangeRange);
            GetComponent<AsteroidSensorInfo>().sensorTimeRange = Random.Range(avgSensorTimeRange - sensorTimeRangeRange, avgSensorRange + sensorTimeRangeRange);
            GetComponent<SpriteRenderer>().color = hasSensorColor;
        }
        else
        {
            GetComponent<AsteroidSensorInfo>().hasSensors = false;
        }
    }
}