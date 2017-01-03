using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum hudState {
	lerpIn, waitStart, lerpOut, playing, fade
}

public class HudController : MonoBehaviour, Pausable {

	public RectTransform levelDisplay, levelDisplayChild, blackBarTop, blackBarBottom;

	public List<Image> UIButtons;

	float blackBarHeight;
	float startTopY;
	float startBottomY;
	float blackBarMid = 0f;

	float midTop = 0, midBottom = 0;


	float lerpInCurrent = 0;
	float lerpInTotal = 0.55f;
	float startX;

	float midX = 0;

	float lerpOutCurrent = 0;
	float lerpOutTotal = 0.55f;
	float endX;


	float endLevelCurrent = 0;
	float endLevelTotal = 0.65f;




	hudState state; //states: in, out, 
	// Use this for initialization
	void Start () {
		/*UIButtons = new List<Image>();
		var rects = Canvas.FindObjectsOfType<RectTransform>();
		foreach(RectTransform r in rects) {
			if (r.CompareTag("LevelDisplay"))
				levelDisplay = r;
			else if (r.CompareTag("LevelDisplayChild"))
				levelDisplayChild = r;
			else if (r.CompareTag("BlackBar")) {
				if (r.anchoredPosition.y > 0) //top bar
					blackBarTop = r;
				else 
					blackBarBottom = r; //bottom bar
			}
		}

		var tempButtons = Canvas.FindObjectsOfType<Image>();
		foreach (Image m in tempButtons) {
			if (m.CompareTag("UIButton"))
				UIButtons.Add(m);
		}*/



		foreach(Image m in UIButtons) {
			m.color = new Color(255, 255, 255, 0); //white but invisible
		}

		blackBarHeight = blackBarTop.rect.height * .425f;
		startTopY = blackBarTop.anchoredPosition.y;
		startBottomY = blackBarBottom.anchoredPosition.y;

		blackBarBottom.GetComponent<RawImage>().color = new Color(0, 0, 0, 1); //made it invisible in editor
		blackBarTop.GetComponent<RawImage>().color = new Color(0, 0, 0, 1);


		state = hudState.lerpIn;
		startX = -750f;
		endX = 750f;

		//set level text
		levelDisplay.GetComponentInParent<Text>().text = "Level " + (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1).ToString();
		levelDisplayChild.GetComponentInParent<Text>().text = levelDisplay.GetComponentInParent<Text>().text;

	}
	
	// Update is called once per frame
	void Update () {
		if (state == hudState.lerpIn) {
			if (lerpInCurrent < lerpInTotal) {
				lerpInCurrent += Time.deltaTime;
			}
			if (lerpInCurrent >= lerpInTotal) {
				lerpInCurrent = lerpInTotal;
				state = hudState.waitStart;
			}

			levelDisplay.anchoredPosition = new Vector2(LeanTween.easeOutBack(startX, 0, lerpInCurrent / lerpInTotal, 0.35f), 0);
			/*
			blackBarTop.anchoredPosition = new Vector2(0, LeanTween.easeOutBack(startTopY, startTopY - blackBarHeight, lerpInCurrent / lerpInTotal, 0.35f));
			blackBarBottom.anchoredPosition = new Vector2(0, LeanTween.easeOutBack(startBottomY, startBottomY + blackBarHeight, lerpInCurrent / lerpInTotal, 0.35f));*/
			blackBarTop.anchoredPosition = new Vector2(0, LeanTween.easeOutSine(-150, 150, lerpInCurrent / lerpInTotal));
			blackBarBottom.anchoredPosition = new Vector2(0, LeanTween.easeOutSine(150, -150, lerpInCurrent / lerpInTotal));

		}
		else if (state == hudState.lerpOut) {
			if (lerpOutCurrent < lerpOutTotal)
				lerpOutCurrent += Time.deltaTime;
			if (lerpOutCurrent >= lerpOutTotal) {
				lerpOutCurrent = lerpOutTotal;
				state = hudState.playing;
			}

			levelDisplay.anchoredPosition = new Vector2(LeanTween.easeInBack(midX, endX, lerpOutCurrent / lerpOutTotal, 0.35f), 0);
			/*blackBarTop.anchoredPosition = new Vector2(0, LeanTween.easeOutBack(startTopY - blackBarHeight, startTopY, lerpOutCurrent / lerpOutTotal, 0.35f));
			blackBarBottom.anchoredPosition = new Vector2(0, LeanTween.easeOutBack(startBottomY + blackBarHeight, startBottomY, lerpOutCurrent / lerpOutTotal, 0.35f));*/
			blackBarTop.anchoredPosition = new Vector2(0, LeanTween.easeOutSine(midTop, 150, lerpOutCurrent / lerpOutTotal));
			blackBarBottom.anchoredPosition = new Vector2(0,LeanTween.easeOutSine(midBottom, -150, lerpOutCurrent / lerpOutTotal));


			foreach(Image i in UIButtons) {
				i.color = new Color(255, 255, 255, LeanTween.linear(0, 0.45f, lerpOutCurrent / lerpOutTotal));
			}
		}
		else if (state == hudState.fade) {
			endLevelCurrent += Time.deltaTime;
			if (endLevelCurrent >= 1f) {
				Debug.Log("Going to next level");
				PauseController.NextLevel(); //GO TO NEXT LEVEL
			}

			blackBarTop.anchoredPosition = new Vector2(0, LeanTween.easeInSine(150, -150, Mathf.Clamp(endLevelCurrent, 0, endLevelTotal) / endLevelTotal));
			blackBarBottom.anchoredPosition = new Vector2(0, LeanTween.easeInSine(-150, 150, Mathf.Clamp(endLevelCurrent, 0, endLevelTotal) / endLevelTotal));
		}

	}

	public void Pause() {
		//Debug.Log("HUD got pause");

	}

	public void Unpause() { //this gets sent first
		//Debug.Log("HUD got unpause");
		if (state == hudState.lerpIn) { //they hit it early, lol
			midX = levelDisplay.anchoredPosition.x;
			state = hudState.lerpOut;
			midTop = blackBarTop.anchoredPosition.y;
			midBottom = blackBarBottom.anchoredPosition.y;
		}
		else if (state == hudState.waitStart) { //hit at wait
			state = hudState.lerpOut;
			midTop = blackBarTop.anchoredPosition.y;
			midBottom = blackBarBottom.anchoredPosition.y;
		}


	}

	public void getButton() {

	}

	public void fadeOut() {
		state = hudState.fade;
	}

}
