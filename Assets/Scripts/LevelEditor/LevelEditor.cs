using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class LevelEditor : Singleton<LevelEditor>
{
    [SerializeField] public int mapSizeX = 1;
    [SerializeField] public int mapSizeY = 1;
    [SerializeField] private HexagonTile hexagonTilePrefab;
    [SerializeField] public float hexagonSize = 1;
    [SerializeField] public int level = 1;
    [SerializeField] public PaintMode paintMode;
    [SerializeField] public HexColor selectedColor = HexColor.Red;
    [SerializeField] public Direction selectedDirection = Direction.None;
    public LevelData levelData;
    Dictionary<Vector2Int, HexagonTile> hexagonTiles = new();

    private Dictionary<HexColor, Color> colorDict = new()
    {
        {HexColor.Cyan, Color.cyan},
        {HexColor.Blue, Color.blue},
        {HexColor.Purple, Color.magenta},
        {HexColor.Red, Color.red},
        {HexColor.Orange, new Color(1f, 0.5f, 0f)},
        {HexColor.Yellow, Color.yellow},
        {HexColor.Green, Color.green}
    };

    private void Awake()
    {
        levelData = new LevelData(new List<HexagonTileData>());
    }

    private void Start()
    {
        LoadLevel();
    }

    public void GenerateMap()
    {
        if (levelData == null || levelData.tileData.Count == 0)
        {
            levelData = new LevelData(new List<HexagonTileData>());
            for (int i = -mapSizeX; i <= mapSizeX; i++)
            {
                for (int j = -mapSizeY; j <= mapSizeY; j++)
                {
                    CreateHexagonTileAt(i, j);
                    HexagonTileData newData = new HexagonTileData(i,j);
                    levelData.tileData.Add(newData);
                }

            }
        }
        else
        {
            foreach (HexagonTileData htd in levelData.tileData)
            {
                CreateHexagonTileAt(htd.q, htd.r);
                ApplyVisualsToTileAt(htd.q, htd.r);
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
        newTile.transform.parent = LevelParent.Instance().transform;
        hexagonTiles[key] = newTile;

        ApplyVisualsToTileAt(q, r);
    }

    public void RemoveHexagonTileAt(int q, int r)
    {
        Debug.Log($"{q}, {r}");
        Vector2Int key = new Vector2Int(q, r);
        Debug.Log(levelData.tileData.FirstOrDefault(x => x.q == q && x.r == r));
        if (hexagonTiles.ContainsKey(key))
        {
            levelData.tileData.Remove(levelData.tileData.FirstOrDefault(x => x.q == q && x.r == r));
            Destroy(hexagonTiles[key].gameObject);
            hexagonTiles.Remove(key);
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
            Debug.Log(jsonFile.text);
            // Normalize legacy data: if no hex, ensure direction is None
            foreach (var td in levelData.tileData)
            {
                if (!td.hasHex)
                {
                    td.direction = Direction.None;
                }
            }
            GenerateMap();
        }
        
        Debug.Log($"Level {level} loaded!");
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

    public bool TryGetTile(int q, int r, out HexagonTile tile)
    {
        return hexagonTiles.TryGetValue(new Vector2Int(q, r), out tile);
    }

    public HexagonTileData GetOrCreateData(int q, int r)
    {
        HexagonTileData data = levelData.tileData.FirstOrDefault(x => x.q == q && x.r == r);
        if (data == null)
        {
            data = new HexagonTileData(q, r);
            levelData.tileData.Add(data);
        }
        return data;
    }

    public void ApplyVisualsToTileAt(int q, int r)
    {
        if (!hexagonTiles.TryGetValue(new Vector2Int(q, r), out HexagonTile tile)) return;
        HexagonTileData data = levelData.tileData.FirstOrDefault(x => x.q == q && x.r == r);
        if (data == null) return;
        Color mappedColor = colorDict.ContainsKey(data.color) ? colorDict[data.color] : Color.white;
        tile.ApplyHexVisual(data.hasHex, data.color, data.direction, mappedColor);
    }
}

public enum PaintMode
{
    Tile,
    Hex
}

public enum HexColor
{
    Cyan,
    Blue,
    Purple,
    Red,
    Orange,
    Yellow,
    Green
}
