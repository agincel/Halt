using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface Pausable
{
	void Pause();
	void Unpause();
	void getButton();
}

public class PauseController : MonoBehaviour {

	public static bool isPaused;
	public static bool hasStarted;
	private Rigidbody2D myRigidbody;
	private bool levelStartPause;

	private Vector2 pastLocation;
	private Vector2 currentLocation;
	private int framesToRestart = 100;
	private int currentRestartFrames = 0;
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		myRigidbody.simulated = false;
		isPaused = false;
		hasStarted = false;
		levelStartPause = false;
		currentLocation = myRigidbody.transform.position;
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

		if (Input.GetButtonDown("Restart")) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart
		}


		if (!hasStarted && !isPaused && !levelStartPause) {
			SendPause(true);
			isPaused = false;
			levelStartPause = true;
		}



		pastLocation = currentLocation;
		currentLocation = myRigidbody.transform.position;
		if (hasStarted && pastLocation == currentLocation && !isPaused) { //AUTO RESTART IF STAND STILL FOR OVER X FRAMES
			currentRestartFrames++;
			if (currentRestartFrames > framesToRestart) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart
			}
		} else if (hasStarted) {
			currentRestartFrames = 0;
		}
	}

	public static void SendPause(bool toPause)
	{
		GameObject[] objectsToPause = GameObject.FindGameObjectsWithTag ("Pause");

		var pausables = new List<Pausable> ();
		foreach (GameObject o in objectsToPause) {
			Pausable[] all = o.GetComponents<Pausable>();
			foreach(Pausable p in all)
				pausables.Add(p);
		}

		foreach(Pausable p in GameObject.FindGameObjectWithTag("MainCamera").GetComponents<Pausable>()) { //special case for main camera
			pausables.Add(p);
		}

		foreach(Pausable p in GameObject.FindGameObjectWithTag("Player").GetComponents<Pausable>()) { //special case for player
			pausables.Add(p);
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
