using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum UIButtonType {
	Restart, Next, Previous, Home, TitleBegin, BackToTitle, DeleteSaves
}

public class UIButtonScript : MonoBehaviour, IPointerClickHandler {

	public UIButtonType type;

	void Update() {
		if (Input.GetButtonDown("Pause")) {
			if (type == UIButtonType.TitleBegin) {
				this.GetComponentInParent<BasicTransition>().changeState(transitionState.closeIn);
				StartCoroutine(NextScreen());
			} else if (type == UIButtonType.BackToTitle) { //hacky, using back to title to start level 1 on space press
				SceneManager.LoadScene(2); //level 1
			}
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (GetComponent<Image> ().color.a >= 0.3f || type == UIButtonType.TitleBegin) {
			if (type == UIButtonType.Restart) {
				PauseController.Restart ();
			} else if (type == UIButtonType.Next) {
				PauseController.NextLevel ();
			} else if (type == UIButtonType.Previous) {
				PauseController.PreviousLevel ();
			} else if (type == UIButtonType.Home) {
				SceneManager.LoadScene(1);
			} else if (type == UIButtonType.TitleBegin) {
				this.GetComponentInParent<BasicTransition>().changeState(transitionState.closeIn);
				StartCoroutine(NextScreen());
			} else if (type == UIButtonType.BackToTitle) {
				this.GetComponentInParent<BasicTransition>().changeState(transitionState.closeIn);
				StartCoroutine(PreviousScreen());
			} else if (type == UIButtonType.DeleteSaves) {
				GameObject.FindGameObjectWithTag ("LevelInfo").GetComponent<LevelSelectController> ().resetSaveData();
				this.GetComponentInParent<BasicTransition>().changeState(transitionState.closeIn);
				StartCoroutine(NextScreen());
			}

			GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayerController>().blip.Play();
		}
	}

	IEnumerator NextScreen() {
		float delay = BasicTransition.tTotal + 0.05f;
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(1);
	}

	IEnumerator PreviousScreen() {
		float delay = BasicTransition.tTotal + 0.05f;
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(0);
	}

}
