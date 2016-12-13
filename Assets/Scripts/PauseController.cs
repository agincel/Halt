using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Pausable
{
	void Pause();
	void Unpause();
}

public class PauseController : MonoBehaviour {

	public static bool isPaused;
	public static bool hasStarted;
	private Rigidbody2D myRigidbody;
	private bool levelStartPause;
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		myRigidbody.simulated = false;
		isPaused = false;
		hasStarted = false;
		levelStartPause = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Pause")) {

			if (!hasStarted) {	
				myRigidbody.simulated = true;
				hasStarted = true;
				SendPause(false);
			} else {
				SendPause(!isPaused);
			}
		}


		if (!hasStarted && !isPaused && !levelStartPause) {
			SendPause(true);
			isPaused = false;
			levelStartPause = true;
		}
	}

	public static void SendPause(bool toPause)
	{
		GameObject[] objectsToPause = GameObject.FindGameObjectsWithTag ("Pause");
		var pausables = new List<Pausable> ();
		foreach (GameObject o in objectsToPause) {
			pausables.Add ((Pausable)o.GetComponent (typeof(Pausable)));
		}

		foreach (Pausable p in pausables) {
			if (!toPause)
				p.Unpause ();
			else
				p.Pause ();
		}
		isPaused = toPause;
	}
}
