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
    public bool shouldAutokill = true;
	private Rigidbody2D myRigidbody;
	private bool levelStartPause;

	private Vector2 pastLocation;
	private Vector2 currentLocation;
	private int framesToRestart = 100;
	private int currentRestartFrames = 0;

	public HudController HUD;

	public bool isInGoal = false;

	public SpeedrunController sc;


	public List<Vector2> tractorVelocities;
	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		myRigidbody.simulated = false;
		isPaused = false;
		hasStarted = false;
		levelStartPause = false;
		currentLocation = myRigidbody.transform.position;
		HUD = FindObjectOfType<Canvas>().GetComponent<HudController>();
		try {
			sc = GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<SpeedrunController>();
		} catch {
			sc = null;
		}
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
		if (Input.GetButtonDown("Next") && !sc.isSpeedrunning) {
			NextLevel();
		}
		if (Input.GetButtonDown("Previous") && !sc.isSpeedrunning) {
			PreviousLevel();
		}


		if (!hasStarted && !isPaused && !levelStartPause) { //pause on level start, but set isPaused to false so the world isn't in negative
			SendPause(true);
			isPaused = false;
			levelStartPause = true;
		}






		pastLocation = currentLocation;
		currentLocation = myRigidbody.transform.position;
		if (hasStarted && pastLocation == currentLocation && !isPaused && !isInGoal && this.GetComponent<SpriteRenderer>().color.a > 0 && shouldAutokill) { //AUTO RESTART IF STAND STILL FOR OVER X FRAMES
			currentRestartFrames++;
			if (currentRestartFrames > framesToRestart) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart
			}
		} else if (hasStarted) {
			currentRestartFrames = 0;
		}
	}

	void FixedUpdate () {
		//receive laser velocities
		if (tractorVelocities.Count > 0) {
			Vector2 setV = new Vector2();
			foreach(Vector2 v in tractorVelocities) {
				setV += v * Time.fixedDeltaTime;
			}

			setV /= tractorVelocities.Count; //average direction being sent
			myRigidbody.velocity = setV;
		}
	}

	public static void test() {
		Debug.Log("Test");
	}

	public static void NextLevel() {

		try
		{
			var localSC = GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<SpeedrunController>();
			if ((((SceneManager.GetActiveScene().buildIndex - 2) + 1) % (SceneManager.sceneCountInBuildSettings - 2)) + 2 < ((SceneManager.GetActiveScene().buildIndex - 2))) { //if we jumped back, we are done
                if (localSC.isSpeedrunning)
                    localSC.finishSpeedrun();
				SceneManager.LoadScene(1); //back to title
			} else {
                if (!localSC.isSpeedrunning)
				    SceneManager.LoadScene((((SceneManager.GetActiveScene().buildIndex - 2) + 1) % (SceneManager.sceneCountInBuildSettings - 2)) + 2); //offset by 2
			}
		} catch {
			SceneManager.LoadScene((((SceneManager.GetActiveScene().buildIndex - 2) + 1) % (SceneManager.sceneCountInBuildSettings - 2)) + 2); //offset by 2
		}

	}

	public static void PreviousLevel() {
        var localSC = GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<SpeedrunController>();
        if (!localSC || !localSC.isSpeedrunning)
        {
            int prevScene = SceneManager.GetActiveScene().buildIndex - 1;
            if (prevScene < 2)
                prevScene = SceneManager.sceneCountInBuildSettings - 1;
            SceneManager.LoadScene(prevScene);
        }
	}

	public static void Restart() {
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

		try {
			pausables.Add(GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<Pausable>()); //music player
		} catch {
			;
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
		try {
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
		catch {
			Debug.Log("LevelSelectController does not exist.");
		}
	}
}
