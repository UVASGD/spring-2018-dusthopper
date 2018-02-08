
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Finished for now
public class DrawPathLinesOptimized : MonoBehaviour {
	//This script draws all possible jumps between asteroids on screen when map is open (the green lines)

	private GameObject[] asteroidList;
	private GameObject[] lines;
	public GameObject container;
	private bool mapOpenLF;
	public float percentToFullyRender; //If two asteroids are within this percentage of the max dist from each other, alpha should be 1. Otherwise, ramp down to zero.
	private Transform mapCenter;
	private int numLinesNeededLF;

	// Use this for initialization
	void Start () {
		asteroidList = GameObject.FindGameObjectsWithTag ("Asteroid");
		asteroidList = asteroidList.OrderBy (asteroid => asteroid.transform.position.x).ToArray (); //initial sort
		lines = new GameObject[asteroidList.Length * (asteroidList.Length - 1) / 2];
		for (int i = 0; i < lines.Length; i++) {
			lines [i] = new GameObject ();
			lines [i].name = "Line" + i.ToString ();
			lines [i].transform.parent = container.transform;
			lines [i].layer = LayerMask.NameToLayer ("UI");
			lines [i].AddComponent (typeof(LineRenderer));
			lines [i].GetComponent<LineRenderer> ().material = new Material(Shader.Find ("Sprites/Default"));
			lines [i].GetComponent<LineRenderer> ().startColor = Color.green;
			lines [i].GetComponent<LineRenderer> ().endColor = Color.green;
			lines [i].GetComponent<LineRenderer> ().startWidth = 0.1f;
			lines [i].GetComponent<LineRenderer> ().endWidth = 0.1f;
			lines [i].GetComponent<LineRenderer> ().enabled = false;
		}
		mapCenter = GameObject.Find ("MapCenter").transform;
		mapOpenLF = false;
	}

	// Update is called once per frame
	void Update () {
		sortByXPositions ();
		if (GameState.mapOpen) {
			DrawPaths ();
		} else if(mapOpenLF){
			for (int i = 0; i < numLinesNeededLF; i++)
			{
				lines [i].GetComponent<LineRenderer> ().enabled = false;
			}
		}
		mapOpenLF = GameState.mapOpen;
	}
		
	void sortByXPositions(){//insertion sort
		for (int i = 0; i < asteroidList.Length - 1; i++) {
			int j = i;
			int k = i + 1;
			while(j >= 0 && asteroidList[j].transform.position.x > asteroidList[k].transform.position.x){
				//swap them
				GameObject tmp = asteroidList [k];
				asteroidList [k] = asteroidList [j];
				asteroidList [j] = tmp;
				k--;
				j--;
			}
		}
	}

	float getAlpha(float dist){
		if (dist < percentToFullyRender*GameState.maxAsteroidDistance) {
			return 1f;
		}
		return 1 - ((dist - percentToFullyRender * GameState.maxAsteroidDistance) / ((1 - percentToFullyRender) * GameState.maxAsteroidDistance));
	}

	void DrawPaths () {
		for (int i = 0; i < numLinesNeededLF; i++)
		{
				lines [i].GetComponent<LineRenderer> ().enabled = false;
		}
		int iter = 0;
		int ast = 0;
		numLinesNeededLF = 0;
		float screenSize = Camera.main.orthographicSize * Screen.width / Screen.height;
		while (ast < asteroidList.Length-1 && asteroidList [ast].transform.position.x < mapCenter.position.x - screenSize - GameState.maxAsteroidDistance) {
			ast++;
		}
		if (ast > 0) {
			ast--;
		}
		while (ast < asteroidList.Length-1 && asteroidList [ast].transform.position.x < mapCenter.position.x + screenSize + GameState.maxAsteroidDistance) {
			int otherAst = ast + 1;
			while (otherAst < asteroidList.Length && (asteroidList [otherAst].transform.position.x - asteroidList [ast].transform.position.x) <= GameState.maxAsteroidDistance) {
				if ((asteroidList [otherAst].transform.position - asteroidList [ast].transform.position).sqrMagnitude <= GameState.maxAsteroidDistance * GameState.maxAsteroidDistance) {
					lines [iter].GetComponent<LineRenderer> ().enabled = true;
					lines [iter].GetComponent<LineRenderer>().SetPosition (0, new Vector3(asteroidList [ast].transform.position.x, asteroidList [ast].transform.position.y, 10f));
					lines [iter].GetComponent<LineRenderer>().SetPosition (1, new Vector3(asteroidList [otherAst].transform.position.x, asteroidList [otherAst].transform.position.y, 10f));
					float a = getAlpha ((asteroidList [ast].transform.position - asteroidList [otherAst].transform.position).magnitude);
					lines [iter].GetComponent<LineRenderer> ().startColor = new Color(0,1,0,a);
					lines [iter].GetComponent<LineRenderer> ().endColor = new Color(0,1,0,a);
					numLinesNeededLF++;
					iter++;
				}
				otherAst++;
			}
			ast++;
		}
	}
}
