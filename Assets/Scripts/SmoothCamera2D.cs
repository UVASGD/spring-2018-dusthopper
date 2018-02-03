using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public Vector3 lastPos;
//	private Transform theAsteroid;
	//public float maxSpeed = 10f;

	void Start () {
		if (target) {
			transform.position = new Vector3 (target.position.x, target.position.y, transform.position.z);
			StartCoroutine (SetDamp (dampTime));
			dampTime = 0f;
		}
	}

	IEnumerator SetDamp (float d) {
		yield return new WaitForEndOfFrame ();
		dampTime = d;
	}

	void Update()
	{
		if(GameState.mapOpen)
		{
			Vector3 temp = /*GameState.asteroid.position +*/ this.gameObject.GetComponent<MoveCameraInMap>().mapCenter.position;
			temp.z = this.transform.position.z;
			this.transform.position = temp;
		}
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if (target)
		{
			if (target.tag == "Player") {
				//transform.parent = GameState.asteroid;
//				theAsteroid = GameState.asteroid;
			}
			transform.rotation = Quaternion.identity;
			Vector3 point = Camera.main.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime, Mathf.Infinity, Time.unscaledDeltaTime);

			lastPos = target.position;
		}
	}
}