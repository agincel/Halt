using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum UIButtonType {
	Restart, Next, Previous, Home, TitleBegin, BackToTitle, DeleteSaves, Speedrun, Settings
}

public class UIButtonScript : MonoBehaviour, IPointerClickHandler {

	public UIButtonType type;
	SpeedrunController sc;

	void Start() {
		if (type == UIButtonType.Speedrun) {
			if (!System.IO.File.Exists(Application.persistentDataPath + @"/speedrunUnlocked.data")) {
				GetComponent<Button>().interactable = false;
				GetComponent<Image>().color = new Color(255, 255, 255, 0);
				GetComponentInChildren<Text>().color = new Color(255, 255, 255, 0);
			} else {
				GetComponent<Button>().interactable = true;
				GetComponent<Image>().color = new Color(255, 255, 255, 1);
				GetComponentInChildren<Text>().color = new Color(0, 0, 0, 1);
			}

			sc = GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<SpeedrunController>();

			string t = "Speedrun:\n";
			t += string.Format("{0}:{1:00}", (int)sc.savedRun.runTotalTime / 60, (int)sc.savedRun.runTotalTime % 60);
			t += "\n\n";
			t += "Best Time: \n";
			t += string.Format("{0}:{1:00}", (int)sc.savedRun.bestTime / 60, (int)sc.savedRun.bestTime % 60);
			GetComponentInChildren<Text>().text = t;
		}
	}

	void Update() {
		if (Input.GetButtonDown("Pause")) { //we hear the space button
			if (type == UIButtonType.TitleBegin) {
				this.GetComponentInParent<BasicTransition>().changeState(transitionState.closeIn);
				StartCoroutine(NextScreen());
                
            } else if (type == UIButtonType.BackToTitle) { //hacky, using back to title to start level 1 on space press
				//SceneManager.LoadScene(2); //level 1
			}
		}

        if (Input.GetButtonDown("Menu") && (type == UIButtonType.Home || type == UIButtonType.BackToTitle))
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
                SceneManager.LoadScene(0);
            else
                SceneManager.LoadScene(1);

            var localSC = GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<SpeedrunController>();
            if (localSC && localSC.isSpeedrunning)
                localSC.clearSpeedrun();
        }

        if (Input.GetButtonDown("Restart") && type == UIButtonType.Restart)
        {
            PauseController.Restart();
        }
        if (Input.GetButtonDown("Next") && type == UIButtonType.Next)
        {
            PauseController.NextLevel();
        }
        if (Input.GetButtonDown("Previous") && type == UIButtonType.Previous)
        {
            PauseController.PreviousLevel();
        }
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (GetComponent<Image> ().color.a >= 0.3f || type == UIButtonType.TitleBegin) {
	
			try {
				sc = GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<SpeedrunController>();
			} catch {
				sc = null;
			}
			if (type == UIButtonType.Restart) {
				PauseController.Restart ();
			} else if (type == UIButtonType.Next) {
				if (!sc.isSpeedrunning)
					PauseController.NextLevel ();
			} else if (type == UIButtonType.Previous) {
				if (!sc.isSpeedrunning)
					PauseController.PreviousLevel ();
			} else if (type == UIButtonType.Home) { //abandon run
				if (sc != null && sc.isSpeedrunning) {
					sc.isSpeedrunning = false;
					if (sc.currentRun.levelCompletionTimes[0] > 0)
						sc.savedRun.levelCompletionTimes = sc.currentRun.levelCompletionTimes; //overwrite splits
					sc.currentRun = sc.savedRun;
				} else if (sc != null){
					sc.showingSpeedrunInfo = false;
				}
				SceneManager.LoadScene(1);
			} else if (type == UIButtonType.TitleBegin) {
				this.GetComponentInParent<BasicTransition>().changeState(transitionState.closeIn);
				sc.showingSpeedrunInfo = false;
				StartCoroutine(NextScreen());
			} else if (type == UIButtonType.BackToTitle) {
				this.GetComponentInParent<BasicTransition>().changeState(transitionState.closeIn);
				StartCoroutine(PreviousScreen());
			} else if (type == UIButtonType.DeleteSaves) {
				GameObject.FindGameObjectWithTag ("LevelInfo").GetComponent<LevelSelectController> ().resetSaveData();
				GameObject.FindGameObjectWithTag ("LevelInfo").GetComponent<SpeedrunController> ().clearSpeedrun();
				this.GetComponentInParent<BasicTransition>().changeState(transitionState.closeIn);
				StartCoroutine(NextScreen());
			} else if (type == UIButtonType.Speedrun) {
				
				this.GetComponentInParent<BasicTransition>().changeState(transitionState.closeIn);
				StartCoroutine(StartSpeedrun());
			}

			try {
				GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayerController>().blip.Play();
			} catch {
				;
			}
		}
	}

	IEnumerator NextScreen() {
		float delay = BasicTransition.tTotal + 0.05f;
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(1);
	}

	IEnumerator PreviousScreen() {
		float delay = BasicTransition.tTotal + 0.05f;
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(0);
	}

	IEnumerator StartSpeedrun() {
		float delay = BasicTransition.tTotal + 0.05f;
		yield return new WaitForSeconds(delay);
		GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<SpeedrunController>().startSpeedrun();
		SceneManager.LoadScene(2);
	}

}
