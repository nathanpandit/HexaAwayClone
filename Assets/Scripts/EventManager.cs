using UnityEngine;
using UnityEngine.Events;

public class EventManager : Singleton<EventManager>
{
    public event UnityAction LevelLost;
    public event UnityAction LevelWon;
    public event UnityAction GamePaused;
    public event UnityAction GameResumed;
    public event UnityAction GameStart;
    public event UnityAction MoveMade;
    public event UnityAction HexFinished;

    public void OnLevelLost() => LevelLost?.Invoke();
    public void OnLevelWon() => LevelWon?.Invoke();
    public void OnGamePaused() => GamePaused?.Invoke();
    public void OnGameResumed() => GameResumed?.Invoke();
    public void OnGameStart() => GameStart?.Invoke();
    public void OnMoveMade() => MoveMade?.Invoke();
    public void OnHexFinished() => HexFinished?.Invoke();

}
