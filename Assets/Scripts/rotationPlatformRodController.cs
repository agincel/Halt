using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationPlatformRodController : MonoBehaviour {

	public GameObject parent;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		rotatingPlatformController myParent = transform.parent.gameObject.GetComponent<rotatingPlatformController>();

	}
}
