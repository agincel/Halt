using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum transitionState {
	closeIn, waitIn, closeOut, waitOut
}

public class BasicTransition : MonoBehaviour {

	public RectTransform blackTop, blackBottom;
	public transitionState tState;

	float topOut = 200, topIn = -200, bottomOut = -200, bottomIn = 200;

	float closeInCurrent = 0, closeOutCurrent = 0;
	public static float tTotal = 0.5f;


	// Use this for initialization
	void Start () {
		blackTop.GetComponent<RawImage>().color = new Color(0, 0, 0, 1);
		blackBottom.GetComponent<RawImage>().color = new Color(0, 0, 0, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (tState == transitionState.closeIn) {
			closeInCurrent += Time.deltaTime;
			if (closeInCurrent >= tTotal) 
			{
				closeInCurrent = tTotal;
				tState = transitionState.waitIn;
			}

			blackTop.anchoredPosition = new Vector2(0, LeanTween.linear(topOut, topIn, closeInCurrent / tTotal));
			blackBottom.anchoredPosition = new Vector2(0, LeanTween.linear(bottomOut, bottomIn, closeInCurrent / tTotal));
		}
		else if (tState == transitionState.closeOut) {
			closeOutCurrent += Time.deltaTime;
			if (closeOutCurrent >= tTotal) {
				closeOutCurrent = tTotal;
				tState = transitionState.waitOut;
			}
			blackTop.anchoredPosition = new Vector2(0, LeanTween.linear(topIn, topOut, closeOutCurrent / tTotal));
			blackBottom.anchoredPosition = new Vector2(0, LeanTween.linear(bottomIn, bottomOut, closeOutCurrent / tTotal));
		}
	}

	public void changeState (transitionState t)
	{
		if (tState != t) {
			tState = t;
			if (tState == transitionState.closeIn)
				closeInCurrent = 0;
			else if (tState == transitionState.closeOut)
				closeOutCurrent = 0;
		}
	}

}
