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

    [SerializeField]
    private float maxSpeed = 1.5f; //how fast can they be going at the start?

    public void InitDefault()
    {
        sensorChance = 0.35f;
        avgSensorRange = 30;
        sensorRangeRange = 15;
        avgSensorTimeRange = 30;
        sensorTimeRangeRange = 15;
        hasSensorColor = Color.yellow;
		print ("initdefault called");
    }

    public void Generate(){
        GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * maxSpeed;
        if (Random.value <= sensorChance)
        {
            GetComponent<AsteroidInfo>().hasSensors = true;
            GetComponent<AsteroidInfo>().sensorRange = Random.Range(avgSensorRange - sensorRangeRange, avgSensorRange + sensorRangeRange);
            GetComponent<AsteroidInfo>().sensorTimeRange = Random.Range(avgSensorTimeRange - sensorTimeRangeRange, avgSensorRange + sensorTimeRangeRange);
            GetComponent<SpriteRenderer>().color = hasSensorColor;
        }
        else
        {
            GetComponent<AsteroidInfo>().hasSensors = false;
        }
    }
}
