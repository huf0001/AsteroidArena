using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class GameplayController : MonoBehaviour 
{
	public int maxLives = 3;
	private int playerLives;
	private Transform player;
	private int score = 0;
	//public float maxTime = 100;
	public Text lifeText;
	public float sceneTime = 0;
	private string lifeOutput = "Lives - ";

	public Text scoreText;
	private string scoreOutput = "Score - ";
    private bool firstCollisionScore = true;

    public Text timerText;
	private string timeOutput = "Timer - ";

	public GameObject ball;

	public GameObject gameOverText;
	public string gameOverScene;

	void Awake()
	{
		PlayerPrefs.SetInt("score", 0);
		playerLives = maxLives;
		lifeText.text = lifeOutput + playerLives;
		scoreText.text = scoreOutput + score;
		lifeText.text = lifeOutput + playerLives;
		player = GameObject.Find ("PhysicsPlayer").transform;
	}

	void Update () 
	{
		sceneTime += Time.deltaTime;
		UpdateTimer();
		//if(maxTime - sceneTime <= 0)
		//	GameOver(gameOverScene);
		if(playerLives <= 0)
			GameOver(gameOverScene);
	}

	public void UpdateScore()
	{
        if (firstCollisionScore)
        {
            score++;
            scoreText.text = scoreOutput + score;
            firstCollisionScore = false;
        }
        else
        {
            firstCollisionScore = true;
        }
    }

	void UpdateTimer()
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
		PlayerPrefs.SetInt("score", (int)score);
		Application.LoadLevel(levelName);
	}
}
