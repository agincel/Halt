using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MouseDownButton : MonoBehaviour, IPointerDownHandler {

	public void OnPointerDown(PointerEventData eventData) {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PauseController> ().ClickPause ();
	}

}
