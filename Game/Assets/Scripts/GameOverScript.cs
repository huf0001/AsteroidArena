using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System;

public class GameOverScript : MonoBehaviour 
{
	public Text scoreText;
    public Text namesParent;
    public Text scoresParent;

    private int playerScore;
    private string playerName;

    private Dictionary<int, string> names = new Dictionary<int, string>();
    private Dictionary<int, int> scores = new Dictionary<int, int>();
    private Text[] nameDisplays;
    private Text[] scoreDisplays;

    private string filename = "Assets/Text Files/Scoreboard.txt";
    
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
        scoreDisplays = scoresParent.GetComponentsInChildren<Text>();
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
                    scores.Add(i + 1, Convert.ToInt32(reader.ReadLine()));
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
        string name = PlayerPrefs.GetString("name");

        if (name != "")
        {
            int score = PlayerPrefs.GetInt("score");

            foreach (KeyValuePair<int, int> p in scores)
            {
                if (score >= p.Value)
                {
                    for (int i = scores.Count; i > p.Key; i--)
                    {
                        names[i] = names[i - 1];
                        scores[i] = scores[i - 1];
                    }

                    scores[p.Key] = score;
                    names[p.Key] = name;
                    break;
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

        foreach (KeyValuePair<int, int> p in scores)
        {
            scoreDisplays[p.Key].text = "" + p.Value;
        }

        scoreText.text = "Score - " + playerScore;
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
                writer.WriteLine(scores[p.Key]);
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
