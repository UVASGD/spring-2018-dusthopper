﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindWaker : MonoBehaviour {

    public bool generateWind = true;
    public GameObject windContainer;
    public GameObject windMaker;
    public int numPoints;
    public float windMakerRadius;

    public float delayBeforeGeneration;
    public float minExistenceTime;
    public float maxExistenceTime;
    public int pregeneratedSteps = 400;

    private static float[] timers;

	// Use this for initialization
	void Awake () {
        // Generate the pool of WindMakers we will be using for the simulation
        for (int x = 0; x <= numPoints; x++)
        {
            GameObject inst = Instantiate(windMaker, Vector3.zero, Quaternion.identity, windContainer.transform);
            inst.SetActive(false);
        }
        // Generate the initial batch of times for wind generation and wind downtime
        float time = 0;
        timers = new float[pregeneratedSteps];
        for (int x = 0; x < pregeneratedSteps; x+=2)
        {
            float existenceTime = Random.Range(minExistenceTime, maxExistenceTime);
            timers[x] = time + existenceTime;
            time += existenceTime;
            float timeBetweenGeneration = Random.Range(0, delayBeforeGeneration);
            timers[x+1] = time + timeBetweenGeneration;
            time += timeBetweenGeneration;
        }
        StartCoroutine(BalladOfGales());

	}
	
	// Update is called once per frame
	void Update () {
        // Checks to see if current cache of steps is running out
        if (GameState.currentWindSimStep == pregeneratedSteps)
        {
            // If so then just restart from the beginning
            GameState.currentWindSimStep = 0;
        }

    }

    IEnumerator BalladOfGales()
    {
        while (generateWind)
        {
            GameState.windExist = true;
            yield return StartCoroutine(generate());
            GameState.windExist = false;
            foreach (Transform child in windContainer.transform)
                child.gameObject.SetActive(false);
            yield return StartCoroutine(MyWaitForSeconds());
        }
    }

    public IEnumerator generate()
    {
        // Generate the endpoints for the wind current
        Vector2 endpoint1 = Random.insideUnitCircle.normalized * 100;
        Vector2 endpoint2 = -endpoint1;

        Vector2 centerOffset = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100));
        endpoint1 += centerOffset;
        endpoint2 += centerOffset;

        // Calculate points 1/3 and 2/3 along the line generated by the endpoints
        Vector2 midpoint1 = .33f * endpoint1 + .66f * endpoint2;
        Vector2 midpoint2 = .66f * endpoint1 + .33f * endpoint2;

        // Calculate a vector perpendicular to the line and random offsets to generate bezier curve
        Vector2 unitPerp = Quaternion.AngleAxis(90, Vector3.forward) * (endpoint2 - endpoint1).normalized;
        float offset1 = Random.Range(-100, 100);
        float offset2 = Random.Range(-100, 100);
        // Generate the newly offset points
        midpoint1 += (unitPerp * offset1);
        midpoint2 += (unitPerp * offset2);

        // Place each WindMaker in the pool at the right point along the curve
        float timeInterval = 1.0f / numPoints;
        Vector2 point;
        Vector2 direction;
        GameObject currChild;
        for (int x = 0; x <= numPoints; x++)
        {
            currChild = transform.GetChild(x).gameObject;
            currChild.SetActive(true);
            float currInterval = x * timeInterval;
            point = (1 - currInterval) * (1 - currInterval) * (1 - currInterval) * endpoint1 +
                        3 * (1 - currInterval) * (1 - currInterval) * currInterval * midpoint1 +
                        3 * (1 - currInterval) * currInterval * currInterval * midpoint2 +
                        currInterval * currInterval * currInterval * endpoint2;
            direction = (3 * (1 - currInterval) * (1 - currInterval) * (midpoint1 - endpoint1) +
                        6 * (1 - currInterval) * currInterval * (midpoint2 - midpoint1) +
                        3 * currInterval * currInterval * (endpoint2 - midpoint2)).normalized;

            currChild.GetComponent<Transform>().position = point;
            currChild.GetComponent<WindMaker>().windDirection = direction;
            currChild.GetComponent<CircleCollider2D>().radius = windMakerRadius;
            currChild.transform.rotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.left, direction), Vector3.forward);
        }

        yield return StartCoroutine(MyWaitForSeconds());
    }

    public static IEnumerator MyWaitForSeconds()
    {
        GameState.currentWindSimStep++;
        while (GameState.time < timers[GameState.currentWindSimStep])
        {
            yield return null;
        }
    }
}
