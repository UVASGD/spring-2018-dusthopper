using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMaker : MonoBehaviour {

    public Vector2 windDirection;
    public float turningForce = 100;
    public float maxSpeed = 10;
    public float damp = .5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Asteroid")
        {
            // Use dot product to project the current asteroid velocity along this one?
            Vector2 currentVelocity = collider.GetComponent<Rigidbody2D>().velocity;
            float angle = Vector3.SignedAngle(currentVelocity, windDirection, Vector3.forward);

            Vector2 perp = Vector3.Cross(currentVelocity, Vector3.forward).normalized;
            Vector2 force = perp * (angle / 180) * turningForce + currentVelocity.normalized * (maxSpeed - currentVelocity.magnitude);
            collider.GetComponent<Rigidbody2D>().AddForce(force * damp);
        }
    }
}
