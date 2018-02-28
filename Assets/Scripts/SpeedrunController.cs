using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct SpeedrunInfo {
	public DateTime startTime;
	public DateTime lastLevelFinish;
	public float[] levelCompletionTimes;
	public DateTime completeTime;
	public float runTotalTime;
	public float bestTime;
}

public class SpeedrunController : MonoBehaviour {

	public bool isSpeedrunning = false;
	public bool showingSpeedrunInfo = true;

	public SpeedrunInfo currentRun;
	public SpeedrunInfo savedRun;

	String filePath;

	// Use this for initialization
	void Start () {
		filePath = Application.persistentDataPath + @"/speedrunInfo.data";


		if (System.IO.File.Exists(filePath)) {
			using (var file = System.IO.File.OpenRead (filePath)) { //if we have the file
				var reader = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ();
				try {
					savedRun = (SpeedrunInfo) reader.Deserialize(file);
				} catch {
					Debug.Log("Error deserializing SpeedrunInfo from file in Start()");
					savedRun = speedrunInfo(5999);
				}
			}
		} else {
			System.IO.File.Create(filePath);
			savedRun = speedrunInfo(5999);
		}
	}

	public void clearSpeedrun () {
		if (System.IO.File.Exists(filePath)) {
			savedRun = speedrunInfo(5999);
			using (var file = System.IO.File.OpenWrite(filePath)) {
				var writer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				writer.Serialize(file, savedRun);
			}
			isSpeedrunning = false;
			showingSpeedrunInfo = false;
		}
	}


	public void startSpeedrun ()
	{
		currentRun = speedrunInfo(savedRun.bestTime);

        isSpeedrunning = true;
        showingSpeedrunInfo = true;
    }

	SpeedrunInfo speedrunInfo(float bestTime) {
		SpeedrunInfo ret = new SpeedrunInfo();
		ret.startTime = System.DateTime.Now;
		ret.lastLevelFinish = ret.startTime;
		ret.levelCompletionTimes = new float[UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 2];

		for (var i = 0; i < ret.levelCompletionTimes.Length; i++) {
			ret.levelCompletionTimes[i] = -1f; //basically null, if -1f overwrite
		}
		ret.runTotalTime = 0;
		ret.bestTime = bestTime;

		return ret;
	}

	public void speedrunUpdateTime() {
		if (isSpeedrunning) {
			for (var i = 0; i < currentRun.levelCompletionTimes.Length; i++) {
				if (currentRun.levelCompletionTimes[i] < 0) {
					currentRun.levelCompletionTimes[i] = (float)(DateTime.Now - currentRun.lastLevelFinish).TotalSeconds;
					Debug.Log(currentRun.levelCompletionTimes[i]);

					currentRun.lastLevelFinish = DateTime.Now;
					i = currentRun.levelCompletionTimes.Length; //break
					break;
				}
			}
		}
	}

	public void finishSpeedrun() {
		currentRun.completeTime = DateTime.Now;

		currentRun.runTotalTime = (float)(currentRun.completeTime - currentRun.startTime).TotalSeconds;

        List<float> times = new List<float>();
        bool hasAdded = false;
        int leaderboardSize = 5;
        for (int i = 0; i < leaderboardSize; i++)
        {
            float f = PlayerPrefs.GetFloat("leader" + i.ToString(), -1f);
            if (!hasAdded && (f <= 0 || currentRun.runTotalTime < f))
            {
                times.Add(currentRun.runTotalTime);
                hasAdded = true;
            }
            times.Add(f);
        }

        for (int i = 0; i < leaderboardSize; i++)
        {
            PlayerPrefs.SetFloat("leader" + i.ToString(), times[i]);
        }


        if (currentRun.runTotalTime < currentRun.bestTime) {
			currentRun.bestTime = currentRun.runTotalTime;
		}

		savedRun = currentRun;

		using (var file = System.IO.File.OpenWrite(filePath)) {
			var writer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			writer.Serialize(file, savedRun);
		}

		isSpeedrunning = false;
		showingSpeedrunInfo = true;
	}


}
