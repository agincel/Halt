using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingPlatformStayFlat : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*Rigidbody2D myRigidbody = (Rigidbody2D)this.GetComponent<Rigidbody2D>();
		myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;*/
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		/*Rigidbody2D myRigidbody = (Rigidbody2D)this.GetComponent<Rigidbody2D>();
		myRigidbody.angularVelocity = 100;//-17;*/
	}
}
