using UnityEngine;

public class TileParent : MonoBehaviour
{
    private static TileParent _instance;

    public static TileParent Instance()
    {
        if (_instance == null)
        {
            _instance = FindFirstObjectByType<TileParent>();
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(TileParent).Name);
                _instance = obj.AddComponent<TileParent>();
            }
        }
        return _instance;
    }
}
