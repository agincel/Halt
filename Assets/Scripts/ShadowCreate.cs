using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCreate : MonoBehaviour {

	// Use this for initialization

    private static float shadowOffset = 0.045f;


	void Start () {
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        GameObject shadow = new GameObject("Shadow");
        SpriteRenderer shadowSr = shadow.AddComponent<SpriteRenderer>();
        if (sr)
        {
            shadowSr.sprite = sr.sprite;
            shadowSr.drawMode = sr.drawMode;
            shadowSr.color = Color.black;
            shadowSr.size = sr.size;
            shadow.transform.localScale = sr.transform.localScale;
            shadow.transform.rotation = sr.transform.rotation;
            shadow.transform.position = new Vector2(this.transform.position.x + shadowOffset, this.transform.position.y - shadowOffset);
            shadowSr.sortingOrder = -1000;
            shadowSr.transform.parent = this.transform;
            shadowSr.transform.tag = "Pause";
            shadowSr.gameObject.AddComponent<colorToggle>();
        }
	}

}
