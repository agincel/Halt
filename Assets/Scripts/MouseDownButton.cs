using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MouseDownButton : MonoBehaviour, IPointerDownHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("Got Mouse Down");
			if (GetComponentInParent<RectTransform>().rect.Contains(Input.mousePosition)) {
				Debug.Log("Mouse Contained in Rect");
				GameObject.FindGameObjectWithTag ("Player").GetComponent<PauseController> ().ClickPause ();
			}
		}*/
	}

	public void OnPointerDown(PointerEventData eventData) {
		Debug.Log("I was clicked down?");
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PauseController> ().ClickPause ();
	}

}
