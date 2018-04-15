using UnityEngine;
using System.Collections;

public class ScrapBehavior : MonoBehaviour {
	public int scrapValue; //1
	private GameObject player;
	public float attractionSpeed;//speed at which scrap is attracted to the player

    void Start(){
		//find player
		player = GameState.player;
	}
	// Update is called once per frame
	void Update ()
	{
		//move toward player with attractive force if player on my asteroid
		if (transform.parent == GameState.asteroid.transform) {
			Vector3 towardPlayer = player.transform.position - transform.position;
			float inv = 1 / towardPlayer.sqrMagnitude;
			transform.position += towardPlayer.normalized * inv * attractionSpeed * GameState.deltaTime;
		}
	}
}

