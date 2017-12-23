using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState {

	public static bool mapOpen;
	public static float time = 0f; //A copy of Time.time which is only incremented when not in a map
	public static float maxAsteroidDistance = 20f;
	public static float secondsPerJump = 5f;
	public static float fieldRadius = 250f;
	public static bool hasSensors = true;
	public static float sensorRange = 30f;
	public static float sensorTimeRange = 30f;
	public static Transform asteroid;
	//public static GameObject player;


}
