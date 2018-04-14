using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateScrapClouds : MonoBehaviour {

    public GameObject ScrapInCloud;

	public void generate(Vector3 center) {

        int scrapToGenerate = 30;
        float radius = 5;
        float maxSpeed = 1f;    //this value is re-used in CameraScrollOut, if change here change there as well
        
        for (int i = 0; i < scrapToGenerate; i++) {

            //eventually modify this to put generated scrap in a folder, rather then just spawn wildly into the main menu
            GameObject inst = Instantiate(ScrapInCloud, center, Quaternion.identity, transform);

            //setting various properties
            inst.GetComponent<StayInRadius>().radius = radius;
            inst.GetComponent<StayInRadius>().center = center;
            inst.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * maxSpeed;
            inst.GetComponent<Rigidbody2D>().freezeRotation = true; //keep scrap from spinning
        }

    }

}
