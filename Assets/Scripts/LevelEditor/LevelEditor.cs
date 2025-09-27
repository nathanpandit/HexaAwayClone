using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : Singleton<LevelEditor>
{
    [SerializeField] public int mapRadius = 1;
    [SerializeField] private HexagonTile hexagonTilePrefab;
    List<HexagonTile> hexagonTiles = new();

    private void Start()
    {
        GenerateHexagonRings();
    }

    private void GenerateHexagonRings()
    {
        for (int i = 1; i <= mapRadius; i++)
        {
            GenerateHexagonRing(i);
        }
    }

    private void GenerateHexagonRing(int i)
    {
        
    }

}
