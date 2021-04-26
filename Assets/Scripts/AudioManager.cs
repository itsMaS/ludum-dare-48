using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float CleanupInterval = 5;

    static GameObject am;
    static AudioSource Music;
    static bool soundMuted;
    public static bool musicMuted;


    static void Initialize()
    {
        if (!am)
        {
            GameObject tmp = new GameObject();
            tmp.name = "Audio Manager";
            tmp.AddComponent<AudioManager>();
        }
    }

    private void Awake()
    {
        if (am)
        {
            Destroy(this);
        }
        else
        {
            am = gameObject;
            StartCoroutine("ClearSounds");
            DontDestroyOnLoad(this);
        }
    }

    public static void PlaySound
        (string soundName, float volume = 1, float pitchDeviation = 0, float pitchOffset = 0)
    {
        Initialize();
        AudioClip sound = Resources.Load<AudioClip>("Audio/" + soundName);
        if (sound)
        {
            AudioSource tmp = am.AddComponent<AudioSource>();
            tmp.clip = sound;
            tmp.mute = soundMuted;
            tmp.volume = volume;
            tmp.pitch = tmp.pitch + pitchOffset + Random.Range(-pitchDeviation, pitchDeviation);

            tmp.Play();
        }
    }

    public static void PlayMusic(string soundName, float volume)
    {
        Initialize();
        AudioClip sound = Resources.Load<AudioClip>("Audio/" + soundName);
        if (sound)
        {
            AudioSource tmp = am.AddComponent<AudioSource>();
            tmp.clip = sound;
            tmp.loop = true;
            tmp.volume = volume;
            tmp.mute = soundMuted || musicMuted;
            Music = tmp;
            tmp.Play();
        }
    }

    public static void ToggleSound()
    {
        soundMuted = !soundMuted;
        foreach (AudioSource el in am.GetComponents<AudioSource>())
        {
            el.mute = soundMuted;
        }
    }
    public static void ToggleMusic()
    {
        musicMuted = !musicMuted;
        Music.mute = musicMuted || soundMuted;
    }

    public static void SetMusic(bool muted)
    {
        musicMuted = muted;
        if (Music)
        {
            Music.mute = musicMuted || soundMuted;
        }
    }

    public static void SetGlobalVolume(float value)
    {
        AudioListener.volume = value;
    }

    IEnumerator ClearSounds()
    {
        while (am)
        {
            foreach (AudioSource el in GetComponents<AudioSource>())
            {
                if (!el.isPlaying)
                {
                    Destroy(el);
                }
            }
            yield return new WaitForSecondsRealtime(CleanupInterval);
        }
    }
}