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

	public Text levelText, diamondText;

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
			GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayerController>().blip.Play();
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
		LevelSelectController lsc = GameObject.FindGameObjectWithTag ("LevelInfo").GetComponent<LevelSelectController> ();

		myLevel = lsc.levels.Find (x => x.index == ID);
		if (ID != 0) //first level edge case
			previousLevel = lsc.levels.Find (x => x.index == ID - 1);
		else {
			previousLevel = empty; //effectively null, a dummy level so level 1 is always unlocked
		}

		if (!previousLevel.completed) {
			GetComponent<Button> ().interactable = false;
		} else {
			GetComponent<Button> ().interactable = true;
		}





		if (ID < lsc.levels.Count) { //is valid level
			levelText.text = (ID + 1).ToString ();
			GetComponent<Image> ().color = new Color (255, 255, 255, 1);


			bool hasCollectedDiamond = lsc.hasCollectedDiamond();

			//diamond text
			if (myLevel.completed && hasCollectedDiamond) { 
				diamondText.color = new Color(0, 0, 0, 1);
				int collectedDiamonds = 0;
				foreach (bool b in myLevel.diamonds)
				{
					if (b == true)
						collectedDiamonds++;
				}

				diamondText.text = collectedDiamonds.ToString() + "/" + myLevel.diamonds.Length.ToString();

				if (collectedDiamonds == myLevel.diamonds.Length){
					GetComponent<Image>().color = new Color(185f / 255f, 1f, 152f / 255f, 1f);
				} else {
					GetComponent<Image>().color = new Color(255, 255, 255, 1);
				}
			}
			else {
				diamondText.color = new Color(0, 0, 0, 0);
			}

			SpeedrunController sc = GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<SpeedrunController>();
			if (sc.showingSpeedrunInfo) { //show split per level
				Debug.Log("Showing Speedrun Info");
				if (sc.savedRun.levelCompletionTimes[ID] > 0) {
					diamondText.color = new Color(0, 0, 0, 1);
					diamondText.text = ((float)((int)(sc.savedRun.levelCompletionTimes[ID] * 1000)) / 1000).ToString(); //round to 3 decimal places
				} else {
					diamondText.color = new Color(0, 0, 0, 0);
				}
			}

			if (hasCollectedDiamond && myLevel.diamonds.Length == 0 && myLevel.completed) {
				GetComponent<Image>().color = new Color(185f / 255f, 1f, 152f / 255f, 1); //if level with no diamonds is completed, it'll fit in with the others and indicate there's nothing else
			}

		}
		else{ //is invalid level
			levelText.text = "";
			GetComponent<Button>().interactable = false;
			GetComponent<Image>().color = new Color(255, 255, 255, 0);
			diamondText.color = new Color(0, 0, 0, 0);
		}
	}

}
