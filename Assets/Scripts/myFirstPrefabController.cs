using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myFirstPrefabController : MonoBehaviour {

	Vector2 startingLocation;
	// Use this for initialization
	void Start () {
		startingLocation = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.y < -10)
			this.transform.position = startingLocation;
	}
}
