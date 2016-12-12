using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class rotatingPlatformController : MonoBehaviour {
	public float rotationRate = 150f;

	public float rotation = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//transform.Rotate(new Vector3(0, 0, 1) * speedMultiplier);
		//rotation += rotationRate * Time.deltaTime;
		Rigidbody2D myRigidbody = (Rigidbody2D)this.GetComponent<Rigidbody2D>();
		myRigidbody.angularVelocity = 17;
	}

	public float getRotation()
	{
		return rotation;
	}
}
