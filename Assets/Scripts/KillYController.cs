﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillYController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D c)
	{
		if (c.gameObject.tag == "Player") {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
		else 
			Destroy(c.gameObject);
	}
}
