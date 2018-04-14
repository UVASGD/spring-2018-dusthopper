using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {
	//This populates the world with asteroids. 

    public GameObject hubPrefab;

    [SerializeField]
	public List<GameObjectAndFloat> asteroids; //this list represents the probability distribution of the different asteroid type
    public List<GameObject> gravityFragmentAsteroids; //contains all the gravity fragment asteroids;
    public List<GameObject> scrapClouds; //contains all generated scrap clouds
    public GameObject container; // Newly created asteroids will be children of this object. For Hierarchy organization. Potential TODO: Create in script as opposed to using reference
    public GameObject scrapCloudContainer;  // Newly created ScapCloudCores will be children of this object
    public int quantity = 200; //how many general asteroids?
    
	public float radius = 200; //how far away from the center can they spawn?
    public float specialAstroidOffsetRadius = 50;

	// Use this for initialization
	void Awake (){
        asteroids.Sort( (p1,p2) => p1.num.CompareTo(p2.num) );
        MakeHub();
        Generate();
	}

    public void MakeHub()
    {
        if(hubPrefab != null)
        {
            GameState.asteroid = Instantiate(hubPrefab, Vector3.zero, Quaternion.identity, container.transform).transform;
        }
    }

    public void Generate(){
        //Generate Asteroids
        
        for (int i = 0; i < quantity; i++)
        {
            float toGenerate = Random.value;
            int asteroidIndex = 0;
            Vector3 pos = Random.insideUnitCircle * radius;
            while (asteroidIndex < asteroids.Capacity){
                if (toGenerate < asteroids[asteroidIndex].num)
                {
                    GameObject inst = GameObject.Instantiate(asteroids[asteroidIndex].goObj, pos, Quaternion.identity, container.transform) as GameObject;
					inst.GetComponent<Rigidbody2D> ().freezeRotation = true; //asteroids rotating is against the law
					inst.name = "Asteroid" + i.ToString();
                    AsteroidInterface ag = inst.GetComponent<AsteroidInterface>();
                    if(ag == null){
                        ag = inst.AddComponent<AsteroidPlain>();
                        ag.InitDefault();
                        Debug.LogWarning("Object prefab in list of asteroid objects to generate() in Generator.cs on GM does not implement AsteroidInterface: " + inst.name);
                    }
                    ag.Generate();
                    inst.transform.parent = container.transform;
                }
                asteroidIndex++;
            }

            //			inst.GetComponent<Rigidbody2D> ().angularVelocity = Random.Range (0f, 50f);

        }
        //Here is where we can spawn special asteroids that might not necessarily be procedural
        Vector3 initialPos = (Random.insideUnitCircle.normalized * radius) + (Random.insideUnitCircle * specialAstroidOffsetRadius);
        GameObject specialInst = GameObject.Instantiate(gravityFragmentAsteroids[0], initialPos, Quaternion.identity, container.transform) as GameObject;
        specialInst.GetComponent<Rigidbody2D>().freezeRotation = true; //asteroids rotating is against the law
        specialInst.GetComponent<StayInRadius>().center = initialPos; //Fixes the location where the asteroid can move
        specialInst.name = "Gravity Fragment Asteroid " + 0.ToString();

        //Currently hardcoded to work with three Gravity Fragment Asteroids, but can be generalized to n Gravity
        //Fragment Asteroids by calculating the angle separating the asteroids as 360/n
        float currentRotation = 120;
        for (int x = 1; x < gravityFragmentAsteroids.Capacity; x++)
        {
            specialInst = GameObject.Instantiate(gravityFragmentAsteroids[x], initialPos, Quaternion.identity, container.transform) as GameObject;
            specialInst.GetComponent<Rigidbody2D>().freezeRotation = true; //asteroids rotating is against the law
            specialInst.transform.RotateAround(Vector3.zero, Vector3.forward, currentRotation);
            specialInst.name = "Gravity Fragment Asteroid " + x.ToString();
            specialInst.GetComponent<StayInRadius>().center = specialInst.transform.position;
            currentRotation += 120;
        }

        //Generating scrap clouds
        for (int i = 0; i < scrapClouds.Capacity; i++) {

            print("Generating scrapCloud #" + (i+1));
            Vector3 pos = Random.insideUnitCircle * radius;

            if (i == 0) {
                pos = Vector3.zero;
            }

            //i am only half sure how this initialization works.
            GameObject cloudInst = GameObject.Instantiate(scrapClouds[i], pos, Quaternion.identity, scrapCloudContainer.transform) as GameObject;

            //Generating surouding scrap cloud
            cloudInst.GetComponent<GenerateScrapClouds>().generate(pos);
            
        }

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

[System.Serializable]
public struct ItemPoolItem
{
	[SerializeField]
	public GameObject obj;
	[SerializeField]
	public float spawnChance;
	[SerializeField]
	public bool uniqueSpawn;
}
