using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Pausable
{
	void Pause();
	void Unpause();
}

public class PauseController : MonoBehaviour {

	private bool isPaused;
	// Use this for initialization
	void Start () {
		isPaused = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Pause")) {
			GameObject[] objectsToPause = GameObject.FindGameObjectsWithTag("Pause");
			var pausables = new List<Pausable>();
			foreach (GameObject o in objectsToPause) {
				pausables.Add((Pausable)o.GetComponent(typeof(Pausable)));
			}

			foreach (Pausable p in pausables) {
				if (isPaused)
					p.Unpause();
				else
					p.Pause();
			}
			isPaused = !isPaused;
		}
	}
}
