using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myFirstPrefabController : MonoBehaviour {

	Vector2 startingLocation;
	Rigidbody2D myRigidbody;
	// Use this for initialization
	void Start () {
		myRigidbody = this.GetComponent<Rigidbody2D>();
		startingLocation = this.transform.position;
		myRigidbody.angularVelocity = 5;
		myRigidbody.AddForce(new Vector2(-100, 5));
	}
	
	// Update is called once per frame
	void Update () {
		/*if (this.transform.position.y < -10)
			this.transform.position = startingLocation;*/
		
	}
}
