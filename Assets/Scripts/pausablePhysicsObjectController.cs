using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pausablePhysicsObjectController : MonoBehaviour, Pausable {

	Rigidbody2D myRigidbody;
	private bool isPaused;

	private float storedAngularVelocity;
	private Vector2 storedLinearVelocity;
	// Use this for initialization
	void Start () {
		myRigidbody = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Pause() {
		storedAngularVelocity = myRigidbody.angularVelocity;
		storedLinearVelocity = myRigidbody.velocity;
		myRigidbody.bodyType = RigidbodyType2D.Static;
		isPaused = true;
	}

	public void Unpause() {
		myRigidbody.bodyType = RigidbodyType2D.Dynamic;
		myRigidbody.velocity = storedLinearVelocity;
		myRigidbody.angularVelocity = storedAngularVelocity;
		isPaused = false;
	}
}
