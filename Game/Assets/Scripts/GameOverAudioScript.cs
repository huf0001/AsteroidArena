using UnityEngine;
using System.Collections;

public class GameOverAudioScript : MonoBehaviour
{
    public AudioClip gameOverSound;
    private AudioSource gameOverSource;

    void Awake()
    {
        gameOverSource = this.gameObject.AddComponent<AudioSource>();

        gameOverSource.loop = true;
        gameOverSource.playOnAwake = true;

        if (gameOverSound != null)
        {
            gameOverSource.clip = gameOverSound;
            gameOverSource.Play();
        }
    }
}