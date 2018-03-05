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
        print("initdefault called");
    }

    public void Awake()
    {
        info = GetComponent<AsteroidInfo>();
    }


    public void Generate(int asteroidNum)
    {
        name = "Asteroid" + asteroidNum.ToString();
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

        int itemCount = (int)(Random.value * info.maxItems);
        for (int x = 0; x < itemCount; x++)
        {
            int itemIndex = 0;
            float toGenerate = Random.value;
            while (itemIndex < info.itempool.Capacity)
            {
                if (toGenerate < info.itempool[itemIndex].num)
                {
					float distFromCenter = Random.Range (GameState.minSpawnDist, info.radius);
					Vector3 pos = Random.insideUnitCircle.normalized * distFromCenter;
                    GameObject inst = GameObject.Instantiate(info.itempool[itemIndex].goObj, transform.position + pos, Quaternion.identity, this.transform) as GameObject;
                    inst.transform.parent = this.transform;
                }
                itemIndex++;
            }
        }
    }
}