using UnityEngine;

public class HexParent : Singleton<HexParent>
{
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
