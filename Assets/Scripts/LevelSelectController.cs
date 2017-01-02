using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct LevelData {
	public string name;
	public bool completed;
	public int collectedDiamonds;
	public int totalDiamonds;
}


public class LevelSelectController : MonoBehaviour {

	List<LevelData> levels;

	// Use this for initialization
	void Start () {
		levels = defineLevels (); //Define the new list of levels






	}
	
	List<LevelData> defineLevels() {
		List<LevelData> ret = new List<LevelData>();

		ret.Add(levelData("Introduction", 1));
		ret.Add(levelData("2xTrouble", 0));
		ret.Add(levelData("Circles", 2));
		ret.Add(levelData("Ellipses", 1));
		ret.Add(levelData("Multitrack", 1));
		ret.Add(levelData("You Keep Moving", 1));
		ret.Add(levelData("Launchpad", 0));
		ret.Add(levelData("Two Birds", 0));
		ret.Add(levelData("Oscillations", 0));
		ret.Add(levelData("Springboard", 1));
		ret.Add(levelData("Bounce House", 0));

		return ret;
	}

	LevelData levelData(string n, int tD) {
		LevelData ret = new LevelData();
		ret.name = n;
		ret.completed = false;
		ret.collectedDiamonds = 0;
		ret.totalDiamonds = tD;

		return ret;
	}
}
