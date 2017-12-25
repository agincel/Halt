using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseController : MonoBehaviour, Pausable {

	public float pulseForce = 400;

	public float pulseTime = 0.25f;
	public float pulseDistance = 0.5f;
	private float currentPulseTime = 0;
	private float currentShrinkTime = 0;

	private bool isPulsing = false;
	private bool isShrinking = false;

	float delay = 0.2f; //can't bounce for 0.2 seconds

	private float initialYScale = 1;

	private bool isPaused = false;

	public Dictionary<GameObject, Collision2D> queuedCollisions;

	public Dictionary<GameObject, float> bounceDelay;

	void Start() {
		initialYScale = transform.localScale.y;
		Debug.Log(initialYScale);
		queuedCollisions = new Dictionary<GameObject, Collision2D>();
		bounceDelay = new Dictionary<GameObject, float>();
	}



	void FixedUpdate() {

		if (!isPaused) {
			if (queuedCollisions.Count > 1) {
				PulseAll(queuedCollisions);
				queuedCollisions.Clear();
			}
		}
		List<GameObject> myKeyList = new List<GameObject>();
		foreach(GameObject g in bounceDelay.Keys) {
			myKeyList.Add(g);
		}

		foreach(GameObject g in myKeyList) {
			bounceDelay[g] -= Time.deltaTime;
			if (bounceDelay[g] <= 0) {
				bounceDelay.Remove(g);
			}
		}

		if (isPulsing && !isPaused) {
			currentPulseTime += Time.deltaTime;
			transform.localScale = new Vector3(transform.localScale.x, initialYScale + LeanTween.easeOutElastic(0, pulseDistance, currentPulseTime / pulseTime), 1);
			if (currentPulseTime >= pulseTime) {
				isPulsing = false;
				isShrinking = true;
				try {
					GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayerController>().pulseOff.Play();
				} catch {
					;
				}
			}
		}

		if (isShrinking && !isPaused) {
			currentShrinkTime += Time.deltaTime;
			transform.localScale = new Vector3(transform.localScale.x, initialYScale + LeanTween.easeOutBack(pulseDistance, 0, currentShrinkTime / (pulseTime / 2)), 1);
			if (currentShrinkTime >= pulseTime / 2) {
				isShrinking = false;
			}
		}
	}

	void Pulse(Collision2D c) {
		if (!bounceDelay.ContainsKey(c.gameObject)) { // !isShrinking && !isPulsing) {
                                                      //Vector2 v = (Vector2)c.gameObject.transform.position - c.contacts[0].point;
            float angle = (this.transform.eulerAngles.z + 90f) % 360;
            Debug.Log(this.transform.eulerAngles);
            Debug.Log(angle);
            Vector2 v = new Vector2(Mathf.Sin(Mathf.Deg2Rad * (90f - angle)), Mathf.Sin(Mathf.Deg2Rad * (angle)));
			c.gameObject.GetComponent<Rigidbody2D>().AddForce(v.normalized * pulseForce);

			bounceDelay.Add(c.gameObject, delay);


			currentPulseTime = 0;
			currentShrinkTime = 0;
			isPulsing = true;
			isShrinking = false;

			try {
				GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayerController>().pulseOn.Play();
			} catch {
				;
			}
		}
	}

	void PulseAll (Dictionary<GameObject, Collision2D> cs)
	{
		if (true) {   //!isShrinking && !isPulsing) {
            bool didPulse = false;
			foreach (KeyValuePair<GameObject, Collision2D> c in cs) {
				if (!bounceDelay.ContainsKey(c.Key) && c.Key != null && c.Value != null) {
                    float angle = (this.transform.eulerAngles.z + 90f) % 360;
                    Vector2 v = new Vector2(Mathf.Sin(Mathf.Deg2Rad * (90f - angle)), Mathf.Sin(Mathf.Deg2Rad * (angle)));
                    c.Value.gameObject.GetComponent<Rigidbody2D> ().AddForce (v.normalized * pulseForce);
					bounceDelay.Add(c.Key, delay); //add them to delay queue
                    didPulse = true;
                }
			}
            if (didPulse)
            {
                currentPulseTime = 0;
                currentShrinkTime = 0;
                isPulsing = true;
                isShrinking = false;
                try
                {
                    GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayerController>().pulseOn.Play();
                }
                catch
                {
                    ;
                }
            }
		}
	}


	void OnCollisionEnter2D(Collision2D c) {	
		if (isPaused) {
			queuedCollisions[c.gameObject] = c;
		} else {
			Pulse(c);
		}
	}

	void OnCollisionStay2D(Collision2D c) {
		if (isPaused && queuedCollisions.ContainsKey(c.gameObject)) {
			queuedCollisions[c.gameObject] = c;
            Debug.Log("Holding collision");
		} else if (!isPaused && queuedCollisions.ContainsKey(c.gameObject)) {
			Pulse(queuedCollisions[c.gameObject]);
            Debug.Log("Releasing Collision");
			queuedCollisions.Remove(c.gameObject);
		}
	}

	void OnCollisionExit2D(Collision2D c) {
		if (queuedCollisions.ContainsKey(c.gameObject))
			queuedCollisions[c.gameObject] = null;
	}

	public void Pause() {
		isPaused = true;
	}

	public void Unpause() {
		isPaused = false;
        PulseAll(queuedCollisions);
	}

	public void getButton() {

	}
}
