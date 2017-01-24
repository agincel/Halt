using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlatformScript : MonoBehaviour {

	float deathCountdown = 0f;
	bool dying = false;



	void Update() {
		if (dying) {
			deathCountdown += Time.deltaTime;
			if (deathCountdown >= 1f) {
				PauseController.Restart();
			}
		}
	}

	void OnCollisionEnter2D (Collision2D c) {
	 	if (c.gameObject.tag == "Player" && !dying) {
	 		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameraController>().shake(0.35f, 0.1f);
	 		c.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0); //make player invisible
	 		c.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; //stop moving

	 		dying = true;
	 		GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayerController>().death.Play();
	 	}
	 }
}
