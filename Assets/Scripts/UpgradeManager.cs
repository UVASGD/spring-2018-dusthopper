using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    private static UpgradeManager _instance;

    public static UpgradeManager Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

	//boolean
	private bool mapUnlocked;

	//Multiplicative
	public float walkSpeedMod;
	private float jumpDistanceMod;
	private float jumpSpeedMod;
	private float timeSeerMod;
	
	//addative
	private int carryCapacityMod;
	private int gulletMod;

	// Use this for initialization
	void Start () {
		Reset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Reset()
	{
		//boolean
		mapUnlocked = false;

		//multiplicative
		walkSpeedMod = 1;
		jumpDistanceMod = 1;
		jumpSpeedMod = 1;
		timeSeerMod = 1;
		
		//addative
		carryCapacityMod = 0;
		gulletMod =0;
	}
}
