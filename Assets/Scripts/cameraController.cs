using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraController : MonoBehaviour, Pausable {
	public bool isFollowing;

	GameObject following;
	float lerpRate = 0.1f;

	private Camera me;

	private Color[] colors;
	private Color[] inverted;

    public Sprite[] bgs;



	float colorLerpTotal = 10f;
	float colorLerpCurrent = 5f;
	bool aToB = false;


	//Canvas elements
	public Text levelDisplay;

	public float shakeDuration = 0f;
	public float shakeIntensity = 0.1f;

	float rootX, rootY;

	// Use this for initialization
	void Start () {
		me = GetComponent<Camera>();
		following = GameObject.FindGameObjectWithTag("Player");
        

		//bg color
		colors = new Color[2];
		inverted = new Color[2];
		colors[0] = new Color(120f / 255f, 135f / 255f, 165f / 255f, 1);
		colors[1] = new Color(141f / 255f, 175f / 255f, 103f / 255f, 1);
		for(var i = 0; i < 2; i++)
			inverted[i] = new Color(1 - colors[i].r, 1 - colors[i].g, 1 - colors[i].b);

		rootX = this.transform.position.x;
		rootY = this.transform.position.y;


        
        this.GetComponentInChildren<SpriteRenderer>().sprite = bgs[(int)Random.Range(0, (float)bgs.Length)];
	}

	void Update() {
		//shake
		if (shakeDuration > 0) {
			shakeDuration -= Time.deltaTime;
			this.transform.position = new Vector3(rootX + ((Random.value * 2) - 1) * shakeIntensity, rootY + ((Random.value * 2) - 1) * shakeIntensity, -100);
		}
	}

	public void shake(float duration, float intensity) {
		shakeDuration = duration;
		shakeIntensity = intensity;
		Debug.Log("Shakey shake");
	}
	
	// Update is called once per physics update
	void FixedUpdate ()
	{
		if (aToB) {
			colorLerpCurrent += Time.deltaTime;
			if (colorLerpCurrent >= colorLerpTotal) {
				colorLerpCurrent = colorLerpTotal;
				aToB = false;
			}
		} else {
			colorLerpCurrent -= Time.deltaTime;
			if (colorLerpCurrent <= 0) {
				colorLerpCurrent = 0;
				aToB = true;
			}
		}

		if (PauseController.isPaused && PauseController.hasStarted) {
			me.backgroundColor = Color.Lerp(inverted[0], inverted[1], colorLerpCurrent / colorLerpTotal);
		} else {
			me.backgroundColor = Color.Lerp(colors[0], colors[1], colorLerpCurrent / colorLerpTotal);
		}

		if (isFollowing) {
			float myX = this.transform.position.x;
			float myY = this.transform.position.y;
			myX += (following.transform.position.x - this.transform.position.x) * lerpRate;
			myY += (following.transform.position.y - this.transform.position.y) * lerpRate;

			this.transform.position = new Vector3 (myX, myY, this.transform.position.z);
		}
	}

	public void Pause() {
		/*if (PauseController.hasStarted)
			me.backgroundColor = inverted;*/
	}

	public void Unpause() {
		//me.backgroundColor = start;
	}

	public void getButton() {

	}

}









