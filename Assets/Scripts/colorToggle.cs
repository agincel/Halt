using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorToggle : MonoBehaviour, Pausable {

	SpriteRenderer mySR;
	private Material invertMat;
	private Material normalMat;

	void Start ()
	{
		mySR = GetComponent<SpriteRenderer> ();
		normalMat = mySR.material;
		invertMat = (Material)Resources.Load("InvertMat");
	}

	public void Pause() {
		if (PauseController.hasStarted)
			mySR.material = invertMat;
	}

	public void Unpause() {
		mySR.material = normalMat;
	}

	public void getButton() {

	}	

}
