using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tweenCases {
	inOutSine, linear, outQuint, outElastic
}

public class movingPlatformController : MonoBehaviour, Pausable {

	public Vector2 deltaDistance;
	public float timeToMove;
	public bool waitForButton;
	public tweenCases tweenCase;
	public bool loop = true;


	private float currentTimeMoved = 0;
	private bool isPaused;
	private bool isReversing = false;
	private bool isDone = false;

	private Rigidbody2D myRigidbody;
	private Vector2 initialLocation;

	SpriteRenderer mySR;


	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
		initialLocation = myRigidbody.position;
		isPaused = true;
		mySR = GetComponent<SpriteRenderer>();

		if (timeToMove == 0)
		{
			Debug.Log("A moving platform's timeToMove is set to 0. Setting to 1.");
			timeToMove = 1;
		}

		if (waitForButton)
			mySR.color = new Color(mySR.color.r, mySR.color.g, mySR.color.b, 0.5f);

	}

	void FixedUpdate ()
	{
		if (!isPaused && !isDone) {
			if (!isReversing)
				currentTimeMoved += Time.deltaTime;
			else
				currentTimeMoved -= Time.deltaTime;

			Vector2 delta = myTween (tweenCase, deltaDistance, currentTimeMoved / timeToMove);
			myRigidbody.MovePosition (new Vector2 (initialLocation.x + delta.x, initialLocation.y + delta.y));

			if (!isReversing && currentTimeMoved >= timeToMove) {
				if (loop)
					isReversing = true;
				else
					isDone = true;
				
			}
			else if (isReversing && currentTimeMoved <= 0)
				isReversing = false;
		}
	}

	private Vector2 myTween(tweenCases t, Vector2 v, float val) {
		Vector2 ret = new Vector2(0, 0);
		if (t == tweenCases.inOutSine) {
			ret.x = LeanTween.easeInOutSine(0, v.x, val);
			ret.y = LeanTween.easeInOutSine(0, v.y, val);
		} else if (t == tweenCases.linear) {
			ret.x = LeanTween.linear(0, v.x, val);
			ret.y = LeanTween.linear(0, v.y, val);
		} else if (t == tweenCases.outQuint) {
			ret.x = LeanTween.easeOutQuint(0, v.x, val);
			ret.y = LeanTween.easeOutQuint(0, v.y, val);
		} else if (t == tweenCases.outElastic) {
			ret.x = LeanTween.easeOutElastic(0, v.x, val, 1, 0.3f);
			ret.y = LeanTween.easeOutElastic(0, v.y, val, 1, 0.3f);
		}

		return ret;
	}

	public void Pause() {
		if (!waitForButton)
			isPaused = true;
	}

	public void Unpause() {
		if (!waitForButton)
			isPaused = false;
	}

	public void getButton() {
		waitForButton = false;
		mySR.color = new Color(mySR.color.r, mySR.color.g, mySR.color.b, 1);
		if (!PauseController.isPaused)
			Unpause();
	}
}
