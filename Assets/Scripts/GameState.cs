using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//This contains a bunch of important global variables.
public static class GameState {

	private static bool inited = false;
	static long dataSaveNumber = 0;

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

		if(!inited)
		{
			Init();
		}

		BinaryFormatter bf = new BinaryFormatter();
		if(!File.Exists(GetPath()))
		{
			File.Create(GetPath());
		}
		FileStream fileStream = File.Open(GetPath(), FileMode.Open);

		Stats data = new Stats();
		data.maxAsteroidDistance = maxAsteroidDistance;
		data.secondsPerJump = secondsPerJump;
		data.playerSpeed = playerSpeed;
		data.maxHunger = maxHunger;

		data.playerPos = SerialVec3.convTo(player.transform.localPosition);

		bf.Serialize(fileStream, data);
		fileStream.Close();
	}

	public static void LoadGame () {

		if(!inited)
		{
			Init();
		}

		if(File.Exists(GetPath()))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fileStream = File.Open(GetPath(), FileMode.Open);
			Stats data = (Stats)bf.Deserialize(fileStream);
			fileStream.Close();
			
			maxAsteroidDistance = data.maxAsteroidDistance;
			secondsPerJump = data.secondsPerJump;
			playerSpeed = data.playerSpeed;
			maxHunger = data.maxHunger;

			player.transform.localPosition = SerialVec3.convFrom(data.playerPos);
		}
		PrintState();
	}

	/*************************************************************************************************/

	public static void ResetGame()
	{
		maxAsteroidDistance = Random.Range (20f, 30f);
		secondsPerJump = Random.Range (1f, 5f);
		playerSpeed = Random.Range (0.2f, 2f);
		maxHunger = Random.Range (20f, 50f);
		player.transform.position = Vector3.zero;
		PrintState();
	}

	private static void PrintState()
	{
		Debug.Log ("Max Asteroid Distance: " + maxAsteroidDistance);
		Debug.Log ("Seconds per jump: " + secondsPerJump);
		Debug.Log ("Player speed: " + playerSpeed);
		Debug.Log ("Max hunger: " + maxHunger);
		Debug.Log ("Player Position: " + player.transform.position);
	}

	private static string GetPath()
	{
		return Application.persistentDataPath + "/stats.dat";
	} 

	private static void Init()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		if(player == null)
		{
			Debug.LogError("No GameObject with Tag Player; Saving and Loading Borked.");
		}
		inited = true;
	}
}

[System.Serializable]
class Stats
{
	public float maxAsteroidDistance;
	public float secondsPerJump;
	public float playerSpeed;
	public float maxHunger;

	public SerialVec3 playerPos;
}