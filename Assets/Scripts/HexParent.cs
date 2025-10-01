using UnityEngine;

public class HexParent : Singleton<HexParent>
{

    int CheckChildCount()
    {
        return transform.childCount;
    }
}
