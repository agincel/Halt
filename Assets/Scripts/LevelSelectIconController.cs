using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectIconController : MonoBehaviour, IPointerClickHandler {

	public int ID;
	LevelData myLevel;
	LevelData previousLevel;

	LevelData empty;

	bool hasInit = false;


	void Start () {
		empty = new LevelData();
		empty.index = -1;
		empty.completed = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!hasInit && GameObject.FindGameObjectsWithTag("LevelInfo").Length > 0) {
			switchPage(0); //init
			hasInit = true;
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (previousLevel.completed && GetComponent<Button>().interactable) {
			this.GetComponentInParent<BasicTransition>().changeState(transitionState.closeIn);
			StartCoroutine(GoToLevel());
		}
	}

	IEnumerator GoToLevel() {
		float delay = BasicTransition.tTotal + 0.05f;
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(ID + 2);
	}

	public void switchPage (int deltaPages) //usually +- 15
	{
		ID += deltaPages;

		myLevel = GameObject.FindGameObjectWithTag ("LevelInfo").GetComponent<LevelSelectController> ().levels.Find (x => x.index == ID);
		if (ID != 0) //first level edge case
			previousLevel = GameObject.FindGameObjectWithTag ("LevelInfo").GetComponent<LevelSelectController> ().levels.Find (x => x.index == ID - 1);
		else {
			previousLevel = empty; //effectively null, a dummy level so level 1 is always unlocked
		}

		if (!previousLevel.completed) {
			GetComponent<Button> ().interactable = false;
		}


		if (ID < GameObject.FindGameObjectWithTag ("LevelInfo").GetComponent<LevelSelectController> ().levels.Count) {
			this.GetComponentInChildren<Text> ().text = (ID + 1).ToString ();
			GetComponent<Image> ().color = new Color (255, 255, 255, 1);
		}
		else{
			this.GetComponentInChildren<Text>().text = "";
			GetComponent<Button>().interactable = false;
			GetComponent<Image>().color = new Color(255, 255, 255, 0);
		}
	}
}
