using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static readonly object lockObject = new object();
    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = FindObjectOfType<AudioManager>();
                    }
                }
            }

            return instance;
        }
    }

    public AudioClip[] audioClips;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string soundName)
    {
        AudioClip clip = Array.Find(audioClips, item => item.name == soundName);
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }
}