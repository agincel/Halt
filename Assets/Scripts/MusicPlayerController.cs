using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerController : MonoBehaviour, Pausable {

	public AudioClip[] Unpaused;
	public AudioClip[] Paused;
	public AudioSource a;
	public AudioSource b;

	public AudioSource blip;
	public AudioSource secret;
	public AudioSource death;
	public AudioSource button;
	public AudioSource pulseOn;
	public AudioSource pulseOff;

	// Use this for initialization
	void Start () {
		if (GameObject.FindGameObjectsWithTag("MusicPlayer").Length > 1) { //if there are more than 1, destroy this one
			GameObject.Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this.gameObject);
		a = this.GetComponent<AudioSource>();

		a.clip = Unpaused[0];
		b.clip = Paused[0];

		a.loop = true;
		b.loop = true;

		a.Play();
		b.Play();

		b.volume = 0;
	}

	public void Pause ()
	{
		if (PauseController.hasStarted) {
			a.volume = 0;
			b.volume = 1;
		}
	}

	public void Unpause() {
		a.volume = 1;
		b.volume = 0;
	}

	public void getButton() {

	}
	

}
