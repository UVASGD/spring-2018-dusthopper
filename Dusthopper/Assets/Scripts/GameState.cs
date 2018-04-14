using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//This contains a bunch of important global variables.
public static class GameState {

	public static bool inited = false;
	static long dataSaveNumber = 0;

	public static bool mapOpen; //whether or not the map is currently open
	public static bool gamePaused; //whether or not the pause menu is open
	public static bool endGame;
	public static bool runActive; //whether or not the player is currently on a run
	public static bool debugMode = true; // If enabled, displays debug buttons and text

	//Game Constants (AKA they shouldn't really be fiddled with)
	/*************************************************************************************************/
	//These are only set from UpdateTime.cs. TODO: move the functionality from UpdateTime.cs into here and delete that script
	public static float time = 0f; //A copy of Time.time which is only incremented when not in a map (for scripts with timing)
	public static float deltaTime = 0f; //A copy of Time.deltaTime which is set to 0 when in a map (for scripts with timing)
	public static float fieldRadius = 250f; //The radius of the circle the game is restricted to
	public static float minSpawnDist = 0.2f; //The radius of the circle in which no items are allowed to spawn.
	public static GameObject player;
	/*************************************************************************************************/


	//Player Stats
	/*************************************************************************************************/
	public static int scrap = 0; // Monies that player possesses
	public static float maxAsteroidDistance = 22f; //The distance the player can jump
	public static float secondsPerJump = 5f; //The time it takes to charge up a jump
	public static float savedMaxAsteroidDistance = 22f; //saved amount
	public static float savedSecondsPerJump = 5f; //saved amount
	public static float playerSpeed = 0.3f; //Speed at which player travels on asteroids
	public static float maxHunger = 60f; //Maximum hunger, or how many seconds until death without replenishing
	public static float hungerLowModifier = 1f; //how much hunger decreases on deltatime
	public static float gravityFragmentCount = 0; //The number of gravity fragments the player has collected (0 to 3)
	/*************************************************************************************************/


	//Player Nonupgradeable Stats
	/*************************************************************************************************/
	public static bool isAlive = true;
	public static bool hungerEnabled = true;
	public static float hunger = 30f;
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
		data.maxAsteroidDistance = savedMaxAsteroidDistance;
		data.secondsPerJump = savedSecondsPerJump;
		data.playerSpeed = playerSpeed;
		data.maxHunger = maxHunger;
		data.hungerLowModifier = hungerLowModifier;
		data.scrap = scrap;

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
			savedMaxAsteroidDistance = data.maxAsteroidDistance;
			savedSecondsPerJump = data.secondsPerJump;
			playerSpeed = data.playerSpeed;
			maxHunger = data.maxHunger;
			hungerLowModifier = data.hungerLowModifier;
			hasSensors = true;
			sensorTimeRange = 30f;
			sensorRange = 30f;

			hunger = maxHunger;
			scrap = data.scrap;

			asteroid = GameObject.FindWithTag("Hub").transform;
			player.transform.position = asteroid.position;
		}
		//PrintState();
	}

	public static void ResetGame()
	{
		maxAsteroidDistance = 22f;
		secondsPerJump = 5f;
		playerSpeed = 0.3f;
		maxHunger = 90f;
		hungerLowModifier = 1f;
		hunger = maxHunger;
		scrap = 0;
		//player.transform.position = Vector3.zero;
//		asteroid = GameObject.FindWithTag("Hub").transform;
//		player.transform.position = GameObject.FindWithTag("Hub").transform.position;
		//PrintState();
	}

	public static void RandomizeStats()
	{
		maxAsteroidDistance = Random.Range (20f, 30f);
		secondsPerJump = Random.Range (2f, 10f);
		playerSpeed = Random.Range (0.2f, 0.5f);
		maxHunger = Random.Range (30f, 120f);
		hunger = maxHunger;
		scrap = Random.Range (0, 10);

		//player.transform.position = Vector3.zero;
		//PrintState();
	}

	private static void PrintState()
	{
		Debug.Log ("Max Asteroid Distance: " + maxAsteroidDistance);
		Debug.Log ("Seconds per jump: " + secondsPerJump);
		Debug.Log ("Player speed: " + playerSpeed);
		Debug.Log ("Max hunger: " + maxHunger);
		Debug.Log ("Scrap: " + scrap);
		Debug.Log ("----------------------------------");
		//Debug.Log ("Player Position: " + player.transform.position);
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
	/*************************************************************************************************/

	/* Upgrade Stats Functions */
	public static void UpgradeMaxAsteroidDistance(float increasePercentage = 0.05f) {
		maxAsteroidDistance *= 1 + increasePercentage;
		savedMaxAsteroidDistance *= 1 + increasePercentage;
	}

	public static void UpgradeSecondsPerJump(float decreasePercentage = 0.05f) {
		secondsPerJump *= 1 - decreasePercentage;
		savedSecondsPerJump *= 1 - decreasePercentage;
	}

	public static void UpgradePlayerSpeed(float increasePercentage = 0.05f) {
		playerSpeed *= 1 + increasePercentage;
	}

	public static void UpgradeMaxHunger(float increasePercentage = 0.05f) {
		maxHunger *= 1 + increasePercentage;
		if (!inited) {
			Init ();
		}
		Hunger hungerScript = player.GetComponent<Hunger>();
		hungerScript.addToHunger (maxHunger * increasePercentage);
	}

	public static void RefreshHunger () {
		hunger = maxHunger;
	}

}
[System.Serializable]
class Stats
{
	public int scrap;

	public float maxAsteroidDistance;
	public float secondsPerJump;
	public float playerSpeed;
	public float maxHunger;
	public float hungerLowModifier;

	public SerialVec3 playerPos;
}