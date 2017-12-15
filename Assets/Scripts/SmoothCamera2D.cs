using UnityEngine;
using System.Collections;

public class SmoothCamera2D : MonoBehaviour {

	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public Vector3 lastPos;

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

	// Update is called once per frame
	void FixedUpdate () 
	{
		if (target && target.tag == "Player")
		{
			transform.parent = GameState.asteroid;

			Vector3 point = Camera.main.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

			lastPos = target.position;
		}

	}
}