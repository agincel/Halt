using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour, Pausable {
	public bool isFollowing;

	GameObject following;
	float lerpRate = 0.1f;

	private Camera me;

	private Color start;
	private Color inverted;
	// Use this for initialization
	void Start () {
		me = GetComponent<Camera>();
		following = GameObject.FindGameObjectWithTag("Player");
		Debug.Log(following.name);

		start = me.backgroundColor;
		inverted = new Color(1 - start.r, 1 - start.g, 1 - start.b);
	}
	
	// Update is called once per physics update
	void FixedUpdate ()
	{
		if (isFollowing) {
			float myX = this.transform.position.x;
			float myY = this.transform.position.y;
			myX += (following.transform.position.x - this.transform.position.x) * lerpRate;
			myY += (following.transform.position.y - this.transform.position.y) * lerpRate;

			this.transform.position = new Vector3 (myX, myY, this.transform.position.z);
		}
	}

	public void Pause() {
		if (PauseController.hasStarted)
			me.backgroundColor = inverted;
	}

	public void Unpause() {
		me.backgroundColor = start;
	}

	public void getButton() {

	}

}









