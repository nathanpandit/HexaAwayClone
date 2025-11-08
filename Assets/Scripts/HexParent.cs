using UnityEngine;

public class HexParent : MonoBehaviour
{
    
    private static HexParent _instance;

    public static HexParent Instance()
    {
        if (_instance == null)
        {
            _instance = FindFirstObjectByType<HexParent>();
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(HexParent).Name);
                _instance = obj.AddComponent<HexParent>();
            }
        }
        return _instance;
    }
    public void CheckChildrenChange()
    {
        // Check GameManager.hexes instead of child count because:
        // 1. Hexes are removed from GameManager.hexes when they exit (before destruction)
        // 2. Unity's Destroy() doesn't immediately remove objects from hierarchy
        // 3. This provides a more reliable check for when all hexes have exited
        if (GameManager.hexes.Count == 0)
        {
            GameManager.LevelWon();
        }
    }
}
