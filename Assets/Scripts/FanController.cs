using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour, Pausable {

	public float strength = 0.1f;

	List<GameObject> objectsMoving;
	Vector2 force;

	Vector2 forceInUp;
	Vector2 forceInDown;

	public float gravityDivisor = 2.5f;

	Color startingColor;

	float initialYScale;


	public float shakeIntensity = 0.1f;
	float lerpTimeCurrent = 0f;
	public float lerpTimeTotal = 0.1f;
	bool isLerpingOut = true;

	float keepInStrength = 50f;
	public bool pullIn = false;

	public bool weak = false;

	// Use this for initialization
	void Start () {
		objectsMoving = new List<GameObject>();
		startingColor = this.GetComponent<SpriteRenderer>().color;
		initialYScale = this.transform.localScale.y;

		force = new Vector2(strength, 0f);
		float rot = transform.eulerAngles.z * Mathf.Deg2Rad; //rotation in radians

		float rotUp = (transform.eulerAngles.z - 90) * Mathf.Deg2Rad;
		float rotDown = (transform.eulerAngles.z + 90) * Mathf.Deg2Rad;

		force = new Vector2(Mathf.Cos(rot) * force.x - Mathf.Sin(rot) * force.y, Mathf.Sin(rot) * force.x + Mathf.Cos(rot) * force.y); //rotated force

		forceInUp = new Vector2(Mathf.Cos(rotUp) * keepInStrength, Mathf.Sin(rotUp) * keepInStrength); //rotated force
		forceInDown = new Vector2(Mathf.Cos(rotDown) * keepInStrength, Mathf.Sin(rotDown) * keepInStrength); //rotated force
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!PauseController.isPaused) {
			foreach (GameObject g in objectsMoving) {
				try {
					if (g.GetComponent<Rigidbody2D> ().velocity.magnitude >= (force * Time.fixedDeltaTime).magnitude && !weak) {
						if (g.tag == "Player") {
							if (!g.GetComponent<PauseController>().tractorVelocities.Contains(force)) {
								g.GetComponent<PauseController>().tractorVelocities.Add(force);
							}
						} else {
							g.GetComponent<Rigidbody2D>().velocity = (force * Time.fixedDeltaTime);
						}
					} else {
						g.GetComponent<Rigidbody2D> ().AddForce(force * Time.fixedDeltaTime);
					}


				} catch {
					;
				}
			}
		}


		if (isLerpingOut) {
			lerpTimeCurrent += Time.deltaTime;
			this.transform.localScale = new Vector3(this.transform.localScale.x, initialYScale + LeanTween.linear(0, shakeIntensity, lerpTimeCurrent / lerpTimeTotal), 1);
			if (lerpTimeCurrent >= lerpTimeTotal){
				isLerpingOut = false;
			}
		} else {
			lerpTimeCurrent -= Time.deltaTime;
			this.transform.localScale = new Vector3(this.transform.localScale.x, initialYScale + LeanTween.linear(0, shakeIntensity, lerpTimeCurrent / lerpTimeTotal), 1);
			if (lerpTimeCurrent <= 0){
				isLerpingOut = true;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D c) {
		objectsMoving.Add(c.gameObject);
		try {
			if (!PauseController.isPaused) {
				c.gameObject.GetComponent<Rigidbody2D>().gravityScale = c.gameObject.GetComponent<Rigidbody2D>().gravityScale / gravityDivisor;
				if (pullIn) {
					Vector2 enteringV = c.gameObject.GetComponent<Rigidbody2D>().velocity;
					c.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(enteringV.x, enteringV.y / 10);
				}
			}
		} catch {
			;
		}
	}

	void OnTriggerExit2D(Collider2D c) {
		objectsMoving.Remove(c.gameObject);
		try {
			if (!PauseController.isPaused) {
				c.gameObject.GetComponent<Rigidbody2D>().gravityScale = c.gameObject.GetComponent<Rigidbody2D>().gravityScale * gravityDivisor;
				if (c.gameObject.tag == "Player" && c.gameObject.GetComponent<PauseController>().tractorVelocities.Contains(force)) {
						c.gameObject.GetComponent<PauseController>().tractorVelocities.Remove(force);
				}
			}
		} catch {
			;
		}
	}

	public void Pause() {
		if (PauseController.hasStarted) {
			this.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);

			foreach(GameObject g in objectsMoving) {
					g.GetComponent<Rigidbody2D>().gravityScale = g.GetComponent<Rigidbody2D>().gravityScale * gravityDivisor;

					if (g.tag == "Player" && g.GetComponent<PauseController>().tractorVelocities.Contains(force)) {
						g.GetComponent<PauseController>().tractorVelocities.Remove(force);
					}
			}
		}
	}

	public void Unpause() {
		this.GetComponent<SpriteRenderer>().color = startingColor;

		foreach(GameObject g in objectsMoving) {
			g.GetComponent<Rigidbody2D>().gravityScale = g.GetComponent<Rigidbody2D>().gravityScale / gravityDivisor;

			if (pullIn) {
				Vector2 enteringV = g.GetComponent<Rigidbody2D>().velocity;
				g.GetComponent<Rigidbody2D>().velocity = new Vector2(enteringV.x, enteringV.y / 10);
			}
		}
	}

	public void getButton() {

	}
}
