using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidInfo : MonoBehaviour {
	//Contained by every asteroid instance
	//Holds some important things about this asteroid and how it appears in the map
	public bool hasSensors;
	public float sensorRange = 30f;
	public float sensorTimeRange = 30f;
	public Sprite mapIcon;
	[HideInInspector]
	public Sprite asteroidSprite;

    public float radius;

    //proc gen sensor stuff
    public Color hasSensorColor;
	[HideInInspector]
	public Color noSensorColor;
    public float sensorChance;
    public int avgSensorRange;
    public int sensorRangeRange;
    public int avgSensorTimeRange;
    public int sensorTimeRangeRange;

    //proc gen food stuff
    public GameObject food;
    public float foodChance;

    [SerializeField]
    public float maxSpeed = 1.5f; //how fast can they be going at the start?
                                  // Use this for initialization

    //proc gen itempool
    [SerializeField]
    public List<ItemPoolItem> itempool; //struct contained in Generator.cs
    public int maxItems;

    void Start() {
		asteroidSprite = GetComponent<SpriteRenderer> ().sprite;
		noSensorColor = GetComponent<SpriteRenderer> ().color;
	}
}
