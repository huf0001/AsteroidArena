using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System;

public class GameOverScript : MonoBehaviour 
{
	public Text endText;
    public Text namesParent;
    public Text progressesParent;
    public Text timesParent;

    private string playerName;
    private int playerProgress;
    private int playerTime;

    private Dictionary<int, string> names = new Dictionary<int, string>();
    private Dictionary<int, int> progresses = new Dictionary<int, int>();
    private Dictionary<int, int> times = new Dictionary<int, int>();
    private Text[] nameDisplays;
    private Text[] progressDisplays;
    private Text[] timeDisplays;

    private string filename = "Scoreboard/Scoreboard.txt";
    private TimeConverter converter = new TimeConverter();
    
    void Awake()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true; // make the cursor useable again.
        
        LoadScoreboard();
        SaveScoreboard();
	}

    private void LoadScoreboard()
    {
        LoadScoreboardObjects();
        ReadScoreboardData();
        UpdateScoreboardData();
        PrintScoreboardData();
    }

    private void LoadScoreboardObjects()
    {
        nameDisplays = namesParent.GetComponentsInChildren<Text>();
        progressDisplays = progressesParent.GetComponentsInChildren<Text>();
        timeDisplays = timesParent.GetComponentsInChildren<Text>();
    }

    private void ReadScoreboardData()
    {
        StreamReader reader = new StreamReader(filename);
        int n = 0;

        try
        {
            if (Int32.TryParse(reader.ReadLine(), out n))
            {
                for (int i = 0; i < 8; i++)
                {
                    names.Add(i + 1, reader.ReadLine());
                    progresses.Add(i + 1, Convert.ToInt32(reader.ReadLine()));
                    times.Add(i + 1, Convert.ToInt32(reader.ReadLine()));
                }
            }
            else
            {
                Debug.Log("<color=orange>" + gameObject.name + ": Error reading " + filename + ".</color>");
            }
        }
        finally
        {
            reader.Close();
        }
    }

    private void UpdateScoreboardData()
    {
        playerName = PlayerPrefs.GetString("name");

        if (playerName != "")
        {
            playerProgress = PlayerPrefs.GetInt("progress");
            playerTime = PlayerPrefs.GetInt("time");

            foreach (KeyValuePair<int, int> p in progresses)
            {
                if (playerProgress > p.Value) 
                {
                    for (int i = progresses.Count; i > p.Key; i--)
                    {
                        names[i] = names[i - 1];
                        progresses[i] = progresses[i - 1];
                        times[i] = times[i - 1];
                    }

                    names[p.Key] = playerName;
                    progresses[p.Key] = playerProgress;
                    times[p.Key] = playerTime;
                    break;
                }
                else if (playerProgress == p.Value)
                {
                    if (playerTime <= times[p.Key])
                    {
                        for (int i = progresses.Count; i > p.Key; i--)
                        {
                            names[i] = names[i - 1];
                            progresses[i] = progresses[i - 1];
                            times[i] = times[i - 1];
                        }

                        names[p.Key] = playerName;
                        progresses[p.Key] = playerProgress;
                        times[p.Key] = playerTime;
                        break;
                    }
                }
            }
        }
    }

    private void PrintScoreboardData()
    {
        foreach (KeyValuePair<int, string> p in names)
        {
            nameDisplays[p.Key].text = p.Value;
        }

        foreach (KeyValuePair<int, int> p in progresses)
        {
            progressDisplays[p.Key].text = "" + p.Value + "%";
        }

        foreach (KeyValuePair<int, int> p in times)
        {
            timeDisplays[p.Key].text = "" + converter.SecondsToDigitalDisplay(p.Value);
        }

        endText.text = "GAME OVER! PROGRESS: " + playerProgress + "%";
    }

    private void SaveScoreboard()
    {
        StreamWriter writer = new StreamWriter(filename);

        try
        {
            writer.WriteLine("1");

            foreach (KeyValuePair<int, string> p in names)
            {
                writer.WriteLine(p.Value);
                writer.WriteLine(progresses[p.Key]);
                writer.WriteLine(times[p.Key]);
            }
        }
        finally
        {
            writer.Close();
        }
    }

	public void StartAgain(string levelName)
	{
		if (levelName == null)
			Debug.Log("<color=orange>"+gameObject.name+": No Scene Name Was given for the StartAgain function!</color>");
		SceneManager.LoadScene(levelName);
	}

    public void QuitGame()
    {
        Application.Quit();
    }
}
