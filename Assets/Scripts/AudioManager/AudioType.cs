using UnityEngine;

public enum SoundType
{
    Win,
    Lose,
    Coin,
    HexFinish,
    None
}

public enum MusicType
{
    StartMenu,
    Game,
    WinScreen,
    None
}


[System.Serializable]
public class Audio
{
    public SoundType soundType;
    public MusicType musicType;
    public AudioClip clip;
}