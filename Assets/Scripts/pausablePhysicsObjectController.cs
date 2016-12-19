using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pausablePhysicsObjectController : MonoBehaviour, Pausable {

	Rigidbody2D myRigidbody;
	private bool isPaused;

	public Vector2 initialLinearVelocity;
	public float initialAngularVelocity;

	private bool hasStarted;

	public bool waitForButton;

	private float storedAngularVelocity;
	private Vector2 storedLinearVelocity;
	// Use this for initialization
	void Start () {
		myRigidbody = this.GetComponent<Rigidbody2D>();
		hasStarted = false;

		myRigidbody.bodyType = RigidbodyType2D.Static;
		isPaused = true;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Pause ()
	{
		if (!waitForButton) {
			storedAngularVelocity = myRigidbody.angularVelocity;
			storedLinearVelocity = myRigidbody.velocity;
			myRigidbody.bodyType = RigidbodyType2D.Static;
			isPaused = true;
		}
	}

	public void Unpause ()
	{
		if (!waitForButton) {
			if (!hasStarted) {
				storedLinearVelocity = initialLinearVelocity;
				storedAngularVelocity = initialAngularVelocity;
				hasStarted = true;
			}

			myRigidbody.bodyType = RigidbodyType2D.Dynamic;
			myRigidbody.velocity = storedLinearVelocity;
			myRigidbody.angularVelocity = storedAngularVelocity;
			isPaused = false;
		}
	}

	public void getButton() {
		waitForButton = false;
		if (!PauseController.isPaused) {
			Unpause();
		}
	}
}
