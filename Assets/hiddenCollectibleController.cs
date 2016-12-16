using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hiddenCollectibleController : MonoBehaviour {
	SpriteRenderer mySprite;
	// Use this for initialization
	void Start () {
		mySprite = this.GetComponent<SpriteRenderer>();
		mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D c) {
		mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 255);
	}
}
