using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct LevelData {
	public int index;
	public string name;
	public bool completed;
	public int collectedDiamonds;
	public int totalDiamonds;
}


public class LevelSelectController : MonoBehaviour {

	public List<LevelData> levels;
	string outputPath;
	int indexTracker = 0;

	// Use this for initialization
	void Start () {

		if (GameObject.FindGameObjectsWithTag("LevelInfo").Length > 1) { //if there are more than 1, destroy this one
			GameObject.Destroy(this);
		}

		outputPath = Application.persistentDataPath + @"/PauseSave.data";
		if (System.IO.File.Exists(outputPath)) {
			using (var file = System.IO.File.OpenRead(outputPath)) {
				var reader = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				levels = (List<LevelData>) reader.Deserialize(file);
			}
		} else {
			levels = defineLevels (); //Define the new list of levels
		}

		DontDestroyOnLoad(this); //object is persistent
	}

	public void updateLevelInLevels(LevelData l) {
		LevelData toUpdate = levels.Find(x => x.index == l.index);
		levels.Remove(toUpdate);
		levels.Add(l);
		saveLevels();
	}

	public void saveLevels() {
		using (var file = System.IO.File.OpenWrite(outputPath)) {
			var writer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			writer.Serialize(file, levels);
		}
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
		ret.index = indexTracker++;
		ret.name = n;
		ret.completed = false;
		ret.collectedDiamonds = 0;
		ret.totalDiamonds = tD;

		return ret;
	}
}
