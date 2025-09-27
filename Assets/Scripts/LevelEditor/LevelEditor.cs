using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = System.Numerics.Vector2;
using Vector3 = UnityEngine.Vector3;

public class LevelEditor : Singleton<LevelEditor>
{
    [SerializeField] public int mapSizeX = 1;
    [SerializeField] public int mapSizeY = 1;
    [SerializeField] private HexagonTile hexagonTilePrefab;
    [SerializeField] public float hexagonSize = 1;
    Dictionary<Vector2Int, HexagonTile> hexagonTiles = new Dictionary<Vector2Int, HexagonTile>();
    
    private static readonly Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int( 1,  0),  // east
        new Vector2Int( 1, -1),  // southeast
        new Vector2Int( 0, -1),  // southwest
        new Vector2Int(-1,  0),  // west
        new Vector2Int(-1,  1),  // northwest
        new Vector2Int( 0,  1)   // northeast
    };

    private void Awake()
    {
        
    }

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        for (int i = -mapSizeX; i <= mapSizeX; i++)
        {
            for (int j = -mapSizeY; j <= mapSizeY; j++)
            {
                CreateHexagonTileAt(i, j);
            }
        }
    }

    public void CreateHexagonTileAt(int q, int r)
    {
        Vector2Int key = new Vector2Int(q, r);
        if (hexagonTiles.ContainsKey(key)) return;
        float x = q * Mathf.Sqrt(3) / 2;
        float y;
        if (q % 2 == 0)
        {
            y = r;
        }
        else
        {
            y = r - 1 / 2f;
        }
        Vector3 position = new Vector3(x * hexagonSize, y * hexagonSize, 0);
        HexagonTile newTile = Instantiate(hexagonTilePrefab, position, Quaternion.identity);
        newTile.name = $"{q} {r}";
        newTile.axialCoordinate = new Vector2Int(q, r);
        newTile.transform.parent = LevelParent.Instance().transform;
        hexagonTiles[key] = newTile;
    }

    public void RemoveHexagonTileAt(int q, int r)
    {
        Debug.Log($"{q}, {r}");
        Vector2Int key = new Vector2Int(q, r);
        if (hexagonTiles.ContainsKey(key))
        {
            Destroy(hexagonTiles[key].gameObject);
            hexagonTiles.Remove(key);
        }
    }

}
