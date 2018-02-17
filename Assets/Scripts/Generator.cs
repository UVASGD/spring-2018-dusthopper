﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {
	//This populates the world with asteroids. 
	//TODO: Every special asteroid and object generation thing will eventually have to be put here

    [SerializeField]
	public List<GameObjectAndFloat> asteroids; //this list represents the probability distribution of the different asteroid type
    public GameObject container; // Newly created asteroids will be children of this object. For Hierarchy organization. Potential TODO: Create in script as opposed to using reference
	public int quantity = 200; //how many general asteroids?
    
	public float radius = 200; //how far away from the center can they spawn?

	// Use this for initialization
	void Awake (){
        asteroids.Sort( (p1,p2) => p1.num.CompareTo(p2.num) );
        generate();
	}

    public void generate(){
        //Generate Asteroids TODO: Put this in Generate() function

        for (int i = 0; i < quantity; i++)
        {
            float toGenerate = Random.value;
            int asteroidIndex = 0;
            Vector3 pos = Random.insideUnitCircle * radius;
            while (asteroidIndex < asteroids.Capacity){
                if (toGenerate < asteroids[asteroidIndex].num)
                {
                    GameObject inst = Instantiate(asteroids[asteroidIndex].goObj, pos, Quaternion.identity) as GameObject;
                    inst.GetComponent<AsteroidGenerate>().generate(i);
                    inst.transform.parent = container.transform;
                }
                asteroidIndex++;
            }

            //			inst.GetComponent<Rigidbody2D> ().angularVelocity = Random.Range (0f, 50f);

        }

        //Here is where we can spawn special asteroids that might not necessarily be procedural
    }
}

[System.Serializable]
public struct GameObjectAndFloat
{
    [SerializeField]
    public GameObject goObj;
    [SerializeField]
    public float num;
}
