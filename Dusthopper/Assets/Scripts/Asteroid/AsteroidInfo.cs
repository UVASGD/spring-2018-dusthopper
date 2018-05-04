using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AsteroidInfo : MonoBehaviour {
	//Contained by every asteroid instance
	//Holds some important things about this asteroid and how it appears in the map
	public bool hasSensors;
	public float sensorRange = 30f;
	public float sensorTimeRange = 30f;
	public Sprite SensorMapIcon;
	[HideInInspector]
	public Sprite asteroidSprite;
    public Sprite NonSenseMapIcon;

	// can see goal arrows?
	public bool goalArrowsVisible;
	public float goalArrowsVisibleChance;

    public float radius;

	//asteroid properties
	public float massFactor = 1f;
	public bool pulledByGrav = false;
	public bool hasGrav = false;

	//asteroid probabilities
	public float chanceGrav;
	public float chancePulledGrav;

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

	// number of green plants on asteroid
	public int greenPlantCount;
	public int bluePlantCount;
	public int yellowPlantCount;
	public int redPlantCount;

    [SerializeField]
    public float maxSpeed = 1.5f; //how fast can they be going at the start?
                                  // Use this for initialization

    //proc gen itempool
    [SerializeField]
    public List<ItemPoolItem> itempool; //struct contained in Generator.cs
	public List<ItemPoolItem> decorationItems;
    public int maxItems;
	public int maxDecorationItems;

    private SpriteRenderer sr;
    private Animator anim;

    void Start() {
        sr = GetComponent<SpriteRenderer>();
		asteroidSprite = sr.sprite;
		noSensorColor = sr.color;
        anim = GetComponent<Animator>();
	}

    public void TriggerPulse() {
        anim.SetTrigger("Pulse");
    }

    //void Update() {
    //    if (GameState.mapOpen) {
    //        float cStart = 10, cEnd = 17, aStart = 0, aEnd = 0;
    //        float c = ((FindObjectOfType<Camera>().orthographicSize - cStart) *
    //            (cEnd - cStart)) / (aEnd - aStart) + aStart;
    //        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, c);
    //    } else if(sr.color.a <1) {
    //        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
    //    }
    //}
}
