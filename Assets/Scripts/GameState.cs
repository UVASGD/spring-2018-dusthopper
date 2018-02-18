using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//This contains a bunch of important global variables.
public static class GameState {

	public static bool mapOpen; //whether or not the map is currently open

	//Game Constants (AKA they shouldn't really be fiddled with)
	/*************************************************************************************************/
	//These are only set from UpdateTime.cs. TODO: move the functionality from UpdateTime.cs into here and delete that script
	public static float time = 0f; //A copy of Time.time which is only incremented when not in a map (for scripts with timing)
	public static float deltaTime = 0f; //A copy of Time.deltaTime which is set to 0 when in a map (for scripts with timing)
	public static float fieldRadius = 250f; //The radius of the circle the game is restricted to
	public static GameObject player;
	/*************************************************************************************************/


	//Upgradeable Player Stats
	/*************************************************************************************************/
	public static float maxAsteroidDistance = 20f; //The distance the player can jump
	public static float secondsPerJump = 5f; //The time it takes to charge up a jump
	public static float playerSpeed = 1f; //Speed at which player travels on asteroids
	public static float maxHunger = 10f; //Maximum hunger, or how many seconds until death without replenishing
	/*************************************************************************************************/


	//Asteroid Specific Stats
	/*************************************************************************************************/
	public static Transform asteroid;//The transform of the asteroid the player is currently sitting on
	public static bool hasSensors = true; //Whether or not the current asteroid has a map
	public static float sensorRange = 30f; //How far you are permitted to move the camera away from the asteroid in the map on this asteroid.
	public static float sensorTimeRange = 30f; //How long you can fast forward in seconds in the map on this asteroid.
	/*************************************************************************************************/

	//During a scheduled jump, this will be set so that you can't screw around with manual jumps while a scheduled jump is taking place
	public static bool manualJumpsDisabled = false;


	//Functions
	/*************************************************************************************************/
	public static void SaveGame () {
		string path = "Assets/Resources/stats.txt";

		StreamWriter writer = new StreamWriter (path, false);
		writer.WriteLine (maxAsteroidDistance);
		writer.WriteLine (secondsPerJump);
		writer.WriteLine (playerSpeed);
		writer.WriteLine (maxHunger);
		writer.Close ();

		//Re-import the file to update the reference in the editor
//		AssetDatabase.ImportAsset(path); 
//		TextAsset asset = Resources.Load("stats.txt") as TextAsset;

		//Print the text from the file
//		Debug.Log(asset.text);
	}

	public static void LoadGame () {
		string path = "Assets/Resources/stats.txt";

		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path); 
		if (!reader.EndOfStream) {
			float.TryParse(reader.ReadLine(), out maxAsteroidDistance);
			float.TryParse(reader.ReadLine(), out secondsPerJump);
			float.TryParse(reader.ReadLine(), out playerSpeed);
			float.TryParse(reader.ReadLine(), out maxHunger);
			reader.Close ();
		}

		Debug.Log ("Max Asteroid Distance: " + maxAsteroidDistance);
		Debug.Log ("Seconds per jump: " + secondsPerJump);
		Debug.Log ("Player speed: " + playerSpeed);
		Debug.Log ("Max hunger: " + maxHunger);
	}
	/*************************************************************************************************/
}
