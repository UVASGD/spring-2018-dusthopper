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
    public int pregeneratedSteps = 100;

    private static float[] existenceTimes;
    private static float[] timeBetweenGenerations;

	// Use this for initialization
	void Start () {
        // Generate the pool of WindMakers we will be using for the simulation
        for (int x = 0; x <= numPoints; x++)
        {
            GameObject inst = Instantiate(windMaker, Vector3.zero, Quaternion.identity, windContainer.transform);
            inst.SetActive(false);
        }
        // Generate the initial batch of times for wind generation and downtime
        float time = 0;
        existenceTimes = new float[pregeneratedSteps];
        timeBetweenGenerations = new float[pregeneratedSteps];
        for (int x = 0; x < 100; x++)
        {
            float existenceTime = Random.Range(minExistenceTime, maxExistenceTime);
            existenceTimes[x] = time + existenceTime;
            time += existenceTime;
            float timeBetweenGeneration = Random.Range(0, delayBeforeGeneration);
            timeBetweenGenerations[x] = time + timeBetweenGeneration;
            time += timeBetweenGeneration;
        }
        StartCoroutine(BalladOfGales());

	}
	
	// Update is called once per frame
	void Update () {
        // Checks to see if current cache of steps is running out
        if (GameState.currentWindSimStep > pregeneratedSteps - 10)
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
            foreach (Transform child in windContainer.transform)
                child.gameObject.SetActive(false);
            GameState.windExist = false;
            yield return StartCoroutine(MyWaitForSecondsDelay());
            GameState.currentWindSimStep++;
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
        }

        yield return StartCoroutine(MyWaitForSecondsExistence());
    }

    public static IEnumerator MyWaitForSecondsExistence()
    {
        while (GameState.time < existenceTimes[GameState.currentWindSimStep])
        {
            yield return null;
        }
    }


    public static IEnumerator MyWaitForSecondsDelay()
    {
        while (GameState.time < timeBetweenGenerations[GameState.currentWindSimStep])
        {
            yield return null;
        }
    }
}
