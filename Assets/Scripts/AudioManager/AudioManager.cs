using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] List<Audio> audios = new();
    public AudioSource MusicSource = new AudioSource();
    public AudioSource SoundSource = new AudioSource();

    protected override void Awake()
    {
        base.Awake();
        MusicSource = gameObject.AddComponent<AudioSource>();
        SoundSource = gameObject.AddComponent<AudioSource>();
        
        // Subscribe to game events
        EventManager.Instance().LevelWon += OnLevelWon;
        EventManager.Instance().HexFinished += OnHexFinished;
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        if (EventManager.Instance() != null)
        {
            EventManager.Instance().LevelWon -= OnLevelWon;
            EventManager.Instance().HexFinished -= OnHexFinished;
        }
    }

    private void OnLevelWon()
    {
        // Play level won sound effect
        PlaySound(SoundType.Win);
    }

    private void OnHexFinished()
    {
        // Play hex finish sound effect
        PlaySound(SoundType.HexFinish);
    }


    public void PlaySound(SoundType audioType)
    {
        Audio audioToPlay = audios.Find(a => a.soundType == audioType);
        
        if (audioToPlay == null)
        {
            Debug.LogWarning($"Audio entry for {audioType} not found in audios list");
            return;
        }
        
        AudioClip clipToPlay = audioToPlay.clip;

        if (clipToPlay == null)
        {
            Debug.LogWarning($"Clip for {audioType} is null");
            return;
        }
        
        SoundSource.clip = clipToPlay;
        
        Debug.Log($"Playing sound {audioType}");
        SoundSource.Play();
    }
    
    public void PlayMusic(MusicType audioType)
    {
        Audio audioToPlay = audios.Find(a => a.musicType == audioType);
        AudioClip clipToPlay = audioToPlay.clip;

        if (clipToPlay == null)
        {
            Debug.LogWarning("Clip to play is null");
            return;
        }
        MusicSource.clip = clipToPlay;
        MusicSource.loop = true;
        Debug.Log($"Playing music {audioType}");
        MusicSource.Play();
    }

    public void StopPlayingMusic()
    {
        MusicSource.Stop();
        Debug.Log($"Stopped playing music: {MusicSource.clip.name}");
    }
}