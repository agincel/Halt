using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerLevelSelect : MonoBehaviour {
    int currentLevel = 1;
    int maxLevel = 34;
    bool speedrun = false;
	// Use this for initialization
	void Start () {
        Text t = GameObject.Find("LeaderboardTimes").GetComponent<Text>();

        List<float> times = new List<float>();
        for (int i = 0; i < 5; i++)
            times.Add(PlayerPrefs.GetFloat("leader" + i.ToString(), -1f));

        string s = "LEADERBOARD:\n";
        foreach (float f in times)
        {
            if (f > 0)
            {
                s += string.Format("{0}:{1:00}", (int)f / 60, (int)f % 60) + "\n";
            }
        }
        t.text = s;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Next") && !speedrun)
        {
            currentLevel += 1;
            if (currentLevel > maxLevel)
                currentLevel = maxLevel;
            UpdateText();
        }
        if (Input.GetButtonDown("Previous") && !speedrun)
        {
            currentLevel -= 1;
            if (currentLevel < 1)
                currentLevel = 1;
            UpdateText();
        }
        if (Input.GetButtonDown("Pause"))
        {
            if (speedrun && currentLevel == 1)
            {
                GameObject.FindGameObjectWithTag("LevelInfo").GetComponent<SpeedrunController>().startSpeedrun();
                SceneManager.LoadScene(2);
            }
            else if (!speedrun)
            {
                SceneManager.LoadScene(1 + currentLevel);
            }
        }
        if (Input.GetButtonDown("Restart"))
        {
            Application.Quit();
        }
        if (Input.GetButtonDown("SpeedrunToggle"))
        {
            speedrun = !speedrun;
            string s = "On";
            if (!speedrun)
                s = "Off";
            else
            {
                currentLevel = 1;
                UpdateText();
            }
            GameObject.Find("ControllerBeginText").GetComponent<Text>().text = "Start: (A)\nExit: (B)\nSpeedrun: " + s + " (X)";
        }
	}

    void UpdateText()
    {
        string s = "";
        if (currentLevel > 1)
            s += "<- ";
        s += currentLevel.ToString();
        if (currentLevel < maxLevel)
            s += " ->";

        this.GetComponent<UnityEngine.UI.Text>().text = s;
    }
}
