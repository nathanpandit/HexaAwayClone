using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
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
    [SerializeField] public int level = 1;
    public LevelData levelData;
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
        levelData = new LevelData(new List<HexagonTileData>());
    }

    private void Start()
    {
        // ResetLevel();
        GenerateMap();
        foreach (HexagonTileData htd in levelData.tileData)
        {
            Debug.Log($"{htd.q}, {htd.r}");
        }
    }

    public void GenerateMap()
    {
        if (levelData == null || levelData.tileData.Count == 0)
        {
            for (int i = -mapSizeX; i <= mapSizeX; i++)
            {
                for (int j = -mapSizeY; j <= mapSizeY; j++)
                {
                    CreateHexagonTileAt(i, j);
                }

            }
        }
        else
        {
            // FIX THIS PART
            List<HexagonTileData> tempData = new();
            tempData = levelData.tileData;
            foreach (HexagonTileData htd in tempData)
            {
                CreateHexagonTileAt(htd.q, htd.r);
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
        HexagonTileData newData = new HexagonTileData(q,r);
        levelData.tileData.Add(newData);
    }

    public void RemoveHexagonTileAt(int q, int r)
    {
        Debug.Log($"{q}, {r}");
        HexagonTileData dataToRemove = levelData.tileData.FirstOrDefault(x => x.q == q && x.r == r);
        Vector2Int key = new Vector2Int(q, r);
        if (dataToRemove != null && hexagonTiles.ContainsKey(key))
        {
            Destroy(hexagonTiles[key].gameObject);
            hexagonTiles.Remove(key);
            levelData.tileData.Remove(dataToRemove);
        }
    }

    public void SaveLevel()
    {
        string saveData = JsonUtility.ToJson(levelData);
        string filePath = Application.dataPath + $"/Resources/Levels/Level_{level}.json";
        System.IO.File.WriteAllText(filePath, saveData);
        Debug.Log($"Level {level} saved!\n{saveData}");
        #if UNITY_EDITOR
        AssetDatabase.Refresh();
        #endif
    }

    public void LoadLevel()
    {
        ResetLevel();
        TextAsset jsonFile = Resources.Load<TextAsset>($"Levels/Level_{level}");

        if (!jsonFile)
        {
            Debug.Log("Creating new level...");
            ResetLevel();
            GenerateMap();
            Debug.Log("New level generated!");
        }
        else
        {
            levelData = JsonUtility.FromJson<LevelData>(jsonFile.text);
            LevelData tempData = levelData;
            GenerateMap();
            levelData = tempData;
        }
    }

    public void ResetLevel()
    {
        foreach (Vector2Int key in hexagonTiles.Keys)
        {
            Destroy(hexagonTiles[key].gameObject);
        }
        hexagonTiles.Clear();
        levelData = new LevelData(new List<HexagonTileData>());
    }

}
