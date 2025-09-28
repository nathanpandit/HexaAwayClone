using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelData
{
    public List<HexagonTileData> tileData;

    public LevelData(List<HexagonTileData> _tileData)
    {
        tileData = _tileData;
    }
}
