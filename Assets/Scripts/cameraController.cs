using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {
	public bool isFollowing;

	GameObject following;
	float lerpRate = 0.1f;
	private Material mat;

	// Use this for initialization
	void Start () {
		following = GameObject.FindGameObjectWithTag("Player");
		Debug.Log(following.name);

		mat = new Material(Shader.Find("ddShaders/dd_Invert"));
	}
	
	// Update is called once per physics update
	void FixedUpdate ()
	{
		if (isFollowing) {
			float myX = this.transform.position.x;
			float myY = this.transform.position.y;
			myX += (following.transform.position.x - this.transform.position.x) * lerpRate;
			myY += (following.transform.position.y - this.transform.position.y) * lerpRate;

			this.transform.position = new Vector3 (myX, myY, this.transform.position.z);
		}
	}

	public void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (PauseController.isPaused) {
			mat.SetTexture ("_MainTex", source);
			Graphics.Blit (source, destination, mat);
		}
	}

}









