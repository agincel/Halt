using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiNextPreviousScript : MonoBehaviour, IPointerClickHandler {

	public bool isNext = false;
	public bool isVisibleByDefault = false;

	bool needsUpdate = false;

	GameObject[] nextPrev;

	void Start() {
		if (!isVisibleByDefault)
			makeInvisible();

		nextPrev = GameObject.FindGameObjectsWithTag("NextPrev"); //self and other button
	}


	void Update ()
	{
		if (needsUpdate) {
			needsUpdate = false;
			LevelSelectController lsc = GameObject.FindGameObjectWithTag ("LevelInfo").GetComponent<LevelSelectController> ();
			if (isNext){

				if (getLowestLevelOnPage() + 15 > lsc.levels.Count - 1) {
					makeInvisible();
				} else {
					Debug.Log((getLowestLevelOnPage() + 15).ToString() + " <= " + (lsc.levels.Count - 1).ToString());
					makeVisible();
				}
			} else {
				if (getLowestLevelOnPage() - 15 < 0) {
					makeInvisible();
				} else {
					makeVisible();
				}
			}
		}
	}


	int getLowestLevelOnPage() {
		GameObject[] buttons = GameObject.FindGameObjectsWithTag("LevelSelectButton");
		int lowestLevel = 99999999; //arbitrarily high

		foreach(GameObject g in buttons) {
			LevelSelectIconController l = g.GetComponent<LevelSelectIconController>();
			if (l.ID < lowestLevel)
				lowestLevel = l.ID;
		}

		return lowestLevel;
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (this.GetComponent<Button> ().enabled) {
			LevelSelectController lsc = GameObject.FindGameObjectWithTag ("LevelInfo").GetComponent<LevelSelectController> ();
			int currentLowLevel = getLowestLevelOnPage ();
			GameObject[] buttons = GameObject.FindGameObjectsWithTag ("LevelSelectButton");

			GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayerController>().blip.Play();

			if (isNext) {
				if (currentLowLevel + 15 <= SceneManager.sceneCountInBuildSettings - 2) {
					foreach (GameObject g in buttons) {
						LevelSelectIconController l = g.GetComponent<LevelSelectIconController> ();
						l.switchPage(15);
					}

					foreach (GameObject g in nextPrev) {
						g.GetComponent<UiNextPreviousScript>().needsUpdate = true;
					}
				}
			} else {
				if (currentLowLevel - 15 >= 0) {
					foreach (GameObject g in buttons) {
						LevelSelectIconController l = g.GetComponent<LevelSelectIconController> ();
						l.switchPage(-15);
					}

					foreach (GameObject g in nextPrev) {
						g.GetComponent<UiNextPreviousScript>().needsUpdate = true;
					}
				}
			}
		}
	}

	void makeInvisible() {
		this.GetComponent<Image>().color = new Color(0, 0, 0, 0);
		this.GetComponentInChildren<Text>().text = "";
		this.GetComponent<Button>().enabled = false;
	}

	void makeVisible() {
		this.GetComponent<Image>().color = new Color(255, 255, 255, 1);
		string text = "<";
		if (isNext)
			text = ">";
		this.GetComponentInChildren<Text>().text = text;
		this.GetComponent<Button>().enabled = true;
	}

}
