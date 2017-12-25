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

	int timesVisited = 1;

	// Use this for initialization
	void Start () {

		if (GameObject.FindGameObjectsWithTag("LevelInfo").Length > 1) { //if there are more than 1, destroy this one

			GameObject[] g = GameObject.FindGameObjectsWithTag("LevelInfo");
			foreach (GameObject o in g) {
				try {
					o.GetComponent<LevelSelectController>().timesVisited += 1;
					if (o.GetComponent<LevelSelectController>().timesVisited == 3) {
						var outputPath = Application.persistentDataPath + @"/speedrunUnlocked.data";
						if (!System.IO.File.Exists(outputPath)) {
							System.IO.File.Create(outputPath);
						}
						GetComponent<SpeedrunController>().showingSpeedrunInfo = true;
					}
				} catch {
					;
				}
			}
			
			GameObject.Destroy(this);
		}

		Screen.fullScreen = true; //REMOVE THIS FOR NON JAM BUILD

		outputPath = Application.persistentDataPath + @"/PauseSave.data";
		if (System.IO.File.Exists(outputPath)) {
			using (var file = System.IO.File.OpenRead(outputPath)) {
				var reader = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				try {
					levels = (List<LevelData>) reader.Deserialize(file);

					Debug.Log("Read in old data");
					if (levels.Count < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 2) {
						Debug.Log("We don't have enough levels");

						GetComponent<SpeedrunController>().clearSpeedrun(); //run is inaccurate


						for (var i = 0; i < (UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 2) - levels.Count; i++) {
							Debug.Log("Adding level " + (i + levels.Count.ToString()));
							levels.Add(levelData(levels.Count + i)); //add in missing levels
							Debug.Log("Added successfully.");
						}
					}
				} catch {
					Debug.Log("Catch block in trying to deserialize file");
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
		Debug.Log("Time to save");

		using (var file = System.IO.File.OpenWrite(outputPath)) {
			Debug.Log("Using file");
			var writer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Debug.Log("Defined writer");
			writer.Serialize(file, levels);
			Debug.Log("Serialized levels to file");
		}

		Debug.Log("Wrote successfully");
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
