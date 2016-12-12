using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class rotatingPlatformController : MonoBehaviour, Pausable {
	private Rigidbody2D myRigidbody;
	public float angularVelocity = 17;
	// Use this for initialization
	void Start () {
		myRigidbody = (Rigidbody2D)this.GetComponent<Rigidbody2D>();
		myRigidbody.angularVelocity = angularVelocity;
	}

	public void Pause()
	{
		myRigidbody.angularVelocity = 0;
	}

	public void Unpause()
	{
		myRigidbody.angularVelocity = angularVelocity;
	}

}
