using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class GameplayController : MonoBehaviour 
{
    //private Transform player;

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
    //private string timeOutput = "Timer - ";

    //public GameObject gameOverText;
    public string gameOverScene;

    void Awake()
	{
		PlayerPrefs.SetInt("progress", 0);
		progressText.text = progressOutput + progress;

        playerLives = maxLives;
        lifeText.text = lifeOutput + playerLives;

        //player = GameObject.Find ("PhysicsPlayer").transform;
    }

	void Update () 
	{
        UpdateTime();

        if (playerLives <= 0)
        {
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
            progress++;
            progressText.text = progressOutput + progress + "%";
            firstCollisionInPair = false;
        }
        else
        {
            firstCollisionInPair = true;
        }
    }

	/*void UpdateTimer()
	{
        string time = "";
        int sec = Mathf.RoundToInt(sceneTime);
        int min = 0;

        while (sec >= 60)
        {
            sec -= 60;
            min++;
        }

        if (min > 9)
        {
            time = time + min;
        }
        else
        {
            time = time + "0";
            time = time + min;
        }

        time = time + ":";

        if (sec > 9)
        {
            time = time + sec;
        }
        else
        {
            time = time + "0";
            time = time + sec;
        }

        timerText.text = time;
	}*/

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
		Application.LoadLevel(levelName);
	}
}
