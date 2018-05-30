using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaAudioScript : MonoBehaviour
{
    public AudioClip arenaSound1;
    public AudioClip arenaSound2;
    public AudioClip arenaSound3;
    private AudioSource arenaSource;
    private List<AudioClip> arenaSounds = new List<AudioClip>();
    int index = 1;

    void Awake()
    {
        arenaSource = this.gameObject.AddComponent<AudioSource>();

        arenaSource.loop = false;
        arenaSource.playOnAwake = true;

        arenaSounds.Add(arenaSound1);
        arenaSounds.Add(arenaSound2);
        arenaSounds.Add(arenaSound3);

        for (int i = 0; i < 3; i++)
        {
            if (arenaSounds[i] != null)
            {
                arenaSource.clip = arenaSound1;
                arenaSource.Play();
                break;
            }
        }
    }

    void Update()
    {
        //if playGravity = true and gravity sound isPlaying = false, play gravity sound
        //update ping timer for gravity 
        //if period > limit (which is 1/2 destructible limit), gravity sound turns off

        if (!arenaSource.isPlaying)
        {
            arenaSource.clip = arenaSounds[index];
            arenaSource.Play();

            index++;

            if (index >= arenaSounds.Count)
            {
                index = 0;
            }
        }
    }
}
