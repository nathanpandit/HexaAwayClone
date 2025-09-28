using UnityEngine;


[System.Serializable]
public class HexagonTileData
{
    public int q;
    public int r;

    public HexagonTileData(int _q, int _r)
    {
        q = _q;
        r = _r;
    }
}