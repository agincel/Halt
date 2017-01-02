using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum UIButtonType {
	Restart, Next, Previous
}

public class UIButtonScript : MonoBehaviour, IPointerClickHandler {

	public UIButtonType type;

	void Update() {

	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (GetComponent<Image> ().color.a >= 0.3f) {
			if (type == UIButtonType.Restart) {
				PauseController.Restart ();
			} else if (type == UIButtonType.Next) {
				PauseController.NextLevel ();
			} else if (type == UIButtonType.Previous) {
				PauseController.PreviousLevel ();
			}
		}
	}
}
