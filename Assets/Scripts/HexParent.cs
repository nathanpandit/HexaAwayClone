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
    void OnTransformChildrenChanged()
    {
        int hexChildren = 0;
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<Hex>() != null)
            {
                hexChildren++;
            }
        }

        if (hexChildren == 0)
        {
            GameManager.LevelWon();
        }
    }
}
