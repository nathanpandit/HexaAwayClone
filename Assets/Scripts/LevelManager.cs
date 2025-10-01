using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    void Awake()
    {
        
    }

    void Start()
    {
        GameManager.StartLevel();
    }
}
