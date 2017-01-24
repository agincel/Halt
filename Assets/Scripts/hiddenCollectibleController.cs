using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hiddenCollectibleController : MonoBehaviour {
	SpriteRenderer mySprite;
	public bool collected = false;
	public int ID;
	// Use this for initialization
	void Start () {
		mySprite = this.GetComponent<SpriteRenderer>();
		mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0);
	}

	void OnTriggerEnter2D (Collider2D c)
	{
		if (c.gameObject.tag == "Player" && !collected) {
			try {
			GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayerController>().secret.Play();
			} catch {
				;
			}
			mySprite.color = new Color (mySprite.color.r, mySprite.color.g, mySprite.color.b, 255);
			collected = true;
			LeanTween.move(this.gameObject, new Vector3(this.transform.position.x, this.transform.position.y + 30, 0), 1f).setEaseInBack().setOvershoot(0.3f);
			PauseController.calculateDiamonds();
		}
	}
}
