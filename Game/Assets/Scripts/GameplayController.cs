using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class GameplayController : MonoBehaviour 
{
    public int maxLives = 3;
    private int playerLives;
    public Text lifeText;
    private string lifeOutput = "Lives - ";

    private int progress = 0;
    public Text progressText;
    private string progressOutput = "Progress - ";
    private bool firstCollisionInPair = true;

    private float sceneTime = 0;
    public Text timerText;
    private TimeConverter converter = new TimeConverter();

    public string gameOverScene;

    void Awake()
	{
		PlayerPrefs.SetInt("progress", 0);
        PlayerPrefs.SetInt("time", 0);
        PlayerPrefs.SetInt("saved", 0);
		progressText.text = progressOutput + progress;

        playerLives = maxLives;
        lifeText.text = lifeOutput + playerLives;
    }

	void Update () 
	{
        UpdateTime();

        if (playerLives <= 0)
        {
            GameOver(gameOverScene);
        }
        else if (progress >= 100)
        {
            progress = 100;
            GameOver(gameOverScene);
        }
	}

    private void UpdateTime()
    {
        sceneTime += Time.deltaTime;
        timerText.text = converter.SecondsToDigitalDisplay(Mathf.RoundToInt(sceneTime));
    }

	public void UpdateProgress()
	{
        if (firstCollisionInPair)
        {

           
            progress = progress + 8;

            progressText.text = progressOutput + progress + "%";
            firstCollisionInPair = false;
        }
        else
        {
            firstCollisionInPair = true;
        }
    }

	public void DecrementLives()
	{
		playerLives --;
		lifeText.text = lifeOutput + playerLives;

        if (playerLives <= 0)
        {
            GameOver(gameOverScene);
        }
	}

	void GameOver(string levelName)
	{
		PlayerPrefs.SetInt("progress", (int)progress);
        PlayerPrefs.SetInt("time", (int)Mathf.RoundToInt(sceneTime));
        PlayerPrefs.SetInt("position", -2);
		Application.LoadLevel(levelName);
	}
}
