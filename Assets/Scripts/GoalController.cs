using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalController : MonoBehaviour {

	bool won = false;
	float pauseCurrent = 0f;
	float pauseTotal = 0.55f;

	Collider2D c;

	void Update() {
		if (won) {
			pauseCurrent += Time.deltaTime;
			if (pauseCurrent > pauseTotal) {
				c.gameObject.GetComponent<PauseController>().HUD.fadeOut();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		/*if (collision.gameObject.tag == "Player")
			SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);*/
		c = collision;
		won = true;
		foreach(Image i in c.gameObject.GetComponent<PauseController>().HUD.UIButtons) {
			i.color = new Color(255, 255, 255, 0); //set them to invisible
		}
		c.gameObject.GetComponent<PauseController>().isInGoal = true;


		//save that you won
		//SceneManager.GetActiveScene().buildIndex
		LevelSelectController lsc = GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<LevelSelectController>();
		LevelData thisLevel = lsc.levels.Find(x => x.index == SceneManager.GetActiveScene().buildIndex - 2);

		thisLevel.completed = true;
		lsc.updateLevelInLevels(thisLevel);
		PauseController.calculateDiamonds(); //handle adding to Diamond totals
	}
}
