using UnityEngine;


[System.Serializable]
public class HexagonTileData
{
    public int q;
    public int r;
    public bool hasHex;
    public Direction direction;

    public HexagonTileData(int _q, int _r, bool _hasHex, Direction _direction)
    {
        q = _q;
        r = _r;
        hasHex = _hasHex;
        if (_hasHex) direction = _direction;
        else direction = Direction.None;
    }
}