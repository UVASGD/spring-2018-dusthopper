using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This contains a bunch of important global variables.
public static class GameState {

	public static bool mapOpen; //whether or not the map is currently open

	//These are only set from UpdateTime.cs. TODO: move the functionality from UpdateTime.cs into here and delete that script
	public static float time = 0f; //A copy of Time.time which is only incremented when not in a map (for scripts with timing)
	public static float deltaTime = 0f; //A copy of Time.deltaTime which is set to 0 when in a map (for scripts with timing)

	public static float maxAsteroidDistance = 20f; //The distance the player can jump
	public static float secondsPerJump = 5f; //The time it takes to charge up a jump
	public static float fieldRadius = 250f; //The radius of the circle the game is restricted to

	public static Transform asteroid;//The transform of the asteroid the player is currently sitting on
	public static bool hasSensors = true; //Whether or not the current asteroid has a map
	public static float sensorRange = 30f; //How far you are permitted to move the camera away from the asteroid in the map on this asteroid.
	public static float sensorTimeRange = 30f; //How long you can fast forward in seconds in the map on this asteroid.

	//During a scheduled jump, this will be set so that you can't screw around with manual jumps while a scheduled jump is taking place
	public static bool manualJumpsDisabled = false;

}
