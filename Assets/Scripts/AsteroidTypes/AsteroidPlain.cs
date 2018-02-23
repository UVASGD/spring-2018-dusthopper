using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPlain : MonoBehaviour , AsteroidInterface {


    //proc gen sensor stuff
    public Color hasSensorColor;
    public float sensorChance;
    public int avgSensorRange;
    public int sensorRangeRange;
    public int avgSensorTimeRange;
    public int sensorTimeRangeRange;

    //proc gen food stuff
    public GameObject food;
    public float foodChance;
   
    [SerializeField]
    private float maxSpeed = 1.5f; //how fast can they be going at the start?
                                   // Use this for initialization

    public void InitDefault()
    {
        sensorChance = 0.35f;
        avgSensorRange = 30;
        sensorRangeRange = 15;
        avgSensorTimeRange = 30;
        sensorTimeRangeRange = 15;
        hasSensorColor = Color.yellow;
        print("initdefault called");
    }


    public void Generate(int asteroidNum)
    {
        name = "Asteroid" + asteroidNum.ToString();
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

        if (Random.value <= foodChance)
        {
            GameObject inst = GameObject.Instantiate(food, transform.position + Vector3.up * .25f, Quaternion.identity, this.transform) as GameObject;
            inst.transform.parent = this.transform;
        }
    }
}
