using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class LevelGenerator : Singleton<LevelGenerator>
{
    private int level;
    private LevelData currentLevelData;
    public Tile tilePrefab;
    public Hex hexPrefab;



    public void GenerateLevel(int l)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Levels/Level_{l}");
        
        if(!jsonFile) Debug.Log("Level does not exist yet!");
        else
        {
            currentLevelData = JsonUtility.FromJson<LevelData>(jsonFile.text);
            foreach (var hdt in currentLevelData.tileData)
            {
                Tile tile = CreateTile(hdt);
                if (hdt.hasHex)
                {
                    CreateHex(hdt, tile);
                }
            }
        }
    }

    Tile CreateTile(HexagonTileData hdt)
    {
        Tile newTile = Instantiate(tilePrefab, AxialToWorld(hdt.q, hdt.r), Quaternion.identity);
        newTile.q = hdt.q;
        newTile.r = hdt.r;
        Vector2Int key = new Vector2Int(hdt.q, hdt.r);
        GameManager.tileDict[key] = newTile;
        newTile.transform.parent = TileParent.Instance().transform;
        return newTile;
    }

    void CreateHex(HexagonTileData hdt, Tile tile)
    {
        Hex newHex = Instantiate(hexPrefab, AxialToWorld(hdt.q, hdt.r), Quaternion.identity);
        newHex.Initialize(tile, hdt.q, hdt.r, hdt.color, hdt.direction);
        GameManager.hexes.Add(newHex);
        newHex.transform.parent = HexParent.Instance().transform;
    }

    Vector2 AxialToWorld(int q, int r)
    {
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

        return new Vector2(x, y);
    }
}
