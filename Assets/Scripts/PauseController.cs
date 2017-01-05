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

	public HudController HUD;

	public bool isInGoal = false;
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		myRigidbody.simulated = false;
		isPaused = false;
		hasStarted = false;
		levelStartPause = false;
		currentLocation = myRigidbody.transform.position;
		HUD = FindObjectOfType<Canvas>().GetComponent<HudController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Pause")) {
			ClickPause();
		}

		if (Input.GetButtonDown("Restart")) {
			Restart();
		}
		if (Input.GetButtonDown("Next")) {
			NextLevel();
		}
		if (Input.GetButtonDown("Previous")) {
			PreviousLevel();
		}


		if (!hasStarted && !isPaused && !levelStartPause) { //pause on level start, but set isPaused to false so the world isn't in negative
			SendPause(true);
			isPaused = false;
			levelStartPause = true;
		}



		pastLocation = currentLocation;
		currentLocation = myRigidbody.transform.position;
		if (hasStarted && pastLocation == currentLocation && !isPaused && !isInGoal) { //AUTO RESTART IF STAND STILL FOR OVER X FRAMES
			currentRestartFrames++;
			if (currentRestartFrames > framesToRestart) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart
			}
		} else if (hasStarted) {
			currentRestartFrames = 0;
		}
	}

	public static void test() {
		Debug.Log("Test");
	}

	public static void NextLevel() {
		SceneManager.LoadScene((((SceneManager.GetActiveScene().buildIndex - 2) + 1) % (SceneManager.sceneCountInBuildSettings - 2)) + 2); //offset by 2
	}

	public static void PreviousLevel() {
		int prevScene = SceneManager.GetActiveScene().buildIndex - 1;
		if (prevScene < 2)
			prevScene = SceneManager.sceneCountInBuildSettings - 1;
		SceneManager.LoadScene(prevScene);
	}

	public static void Restart() {
		calculateDiamonds();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart
	}

	public void ClickPause() {

		if (isInGoal) {
			HUD.fadeOut();
			return;
		}

		if (!hasStarted) {	
			myRigidbody.simulated = true;
			hasStarted = true;
			SendPause(false);
		} else {
			SendPause(!isPaused);
		}
	}

	public void SendPause(bool toPause)
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

		if (HUD != null)
			pausables.Add((Pausable)HUD);

		foreach (Pausable p in pausables) {
			if (!toPause)
				p.Unpause ();
			else
				p.Pause ();
		}
		isPaused = toPause;
	}

	public static void calculateDiamonds() {
		LevelSelectController lsc = GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<LevelSelectController>();
		LevelData thisLevel = lsc.levels.Find(x => x.index == SceneManager.GetActiveScene().buildIndex - 2);

		//TODO DIAMONDS
		GameObject[] diamonds = GameObject.FindGameObjectsWithTag("Diamond");

		if (thisLevel.diamonds.Length != diamonds.Length) {
			thisLevel.diamonds = new bool[diamonds.Length]; //set the boolean array
		}


		foreach (GameObject g in diamonds) {
			hiddenCollectibleController d = g.GetComponent<hiddenCollectibleController>();
			if (d.collected)
				thisLevel.diamonds[d.ID] = true; //set to true if not already collected
		}

		lsc.updateLevelInLevels(thisLevel);
	}
}
