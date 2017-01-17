using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct LevelData {
	public int index;
	public bool completed;
	public bool[] diamonds;
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
				try {
					levels = (List<LevelData>) reader.Deserialize(file);
				} catch {
					levels = defineLevels(); //if I went and changed the save format
				}
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

		for(int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 2; i++) {
			ret.Add(levelData(i));
		}

		return ret;
	}

	LevelData levelData(int ind) {
		LevelData ret = new LevelData();
		ret.index = ind;
		ret.completed = false;
		ret.diamonds = new bool[0]; //empty, gets replaced upon level completion

		return ret;
	}

	public void resetSaveData() {
		levels = defineLevels();
		saveLevels();
	}

	public bool hasCollectedDiamond() {
		bool ret = false;
		foreach (LevelData ld in levels) {
			foreach (bool b in ld.diamonds) {
				if (b)
					ret = true;
			}
		}

		return ret;
	}
}
