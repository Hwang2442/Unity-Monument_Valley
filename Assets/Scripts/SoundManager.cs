using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<Sound> sounds = new List<Sound>();

    AudioSource bgm;
    AudioSource effect;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        bgm = gameObject.AddComponent<AudioSource>();
        effect = gameObject.AddComponent<AudioSource>();

        bgm.playOnAwake = false;
        effect.playOnAwake = false;

        play("IntroBGM");
    }

    public void play(string audioName, float volume = 1.0f)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (audioName == sounds[i].audio.name)
            {
                if (sounds[i].isBGM)
                {
                    bgm.Stop();

                    bgm.clip = sounds[i].audio;
                    bgm.loop = true;
                    bgm.volume = volume;

                    bgm.Play();
                }
                else
                {
                    effect.PlayOneShot(sounds[i].audio, volume);
                }

                break;
            }
        }
    }

    public void stop(string audioName)
    {
        for (int i = 0; i < sounds.Count; i++)
        {
            if (audioName == sounds[i].audio.name)
            {
                if (sounds[i].isBGM)
                {
                    bgm.Stop();
                }
                else
                {
                    effect.Stop();
                }
            }
        }
    }
}

[System.Serializable]
public class Sound
{
    public AudioClip audio;
    public bool isBGM = false;
}