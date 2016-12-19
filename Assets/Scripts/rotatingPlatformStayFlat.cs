using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingPlatformStayFlat : MonoBehaviour {
	void FixedUpdate () {
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
	}
}
