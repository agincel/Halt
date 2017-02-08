using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonController : MonoBehaviour {

	public GameObject[] affects;
	private bool isPressed;

	public bool onlyPlayerCanPress = false;
	// Use this for initialization
	void Start () {
		isPressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter2D (Collider2D c)
	{
		if (!isPressed && (!onlyPlayerCanPress || c.tag == "Player")) {
			foreach (GameObject g in affects) {
				Pausable p = (Pausable)g.GetComponent (typeof(Pausable));
				p.getButton ();
			}



			SpriteRenderer s = GetComponent<SpriteRenderer> ();
			s.color = new Color (s.color.r, s.color.g, s.color.b, 0); //set to invisible
			isPressed = true;

			GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayerController>().button.Play();
		}
	}
}
