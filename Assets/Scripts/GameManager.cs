using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GameManager
{
    public static int level = 1;
    public static float unitDuration = 0.1f;
    public static Dictionary<Vector2Int, Tile> tileDict = new();
    public static List<Hex> hexes = new();

    public static Dictionary<HexColor, Color> colorDict = new()
    {
        {HexColor.Cyan, Color.cyan},
        {HexColor.Blue, Color.blue},
        {HexColor.Purple, Color.magenta},
        {HexColor.Red, Color.red},
        {HexColor.Orange, new Color(1f, 0.5f, 0f)},
        {HexColor.Yellow, Color.yellow},
        {HexColor.Green, Color.green}
    };


    public static void StartLevel()
    {
        LevelGenerator.Instance().GenerateLevel(level);
    }

    public static void LevelWon()
    {
        Debug.Log("Level Won!");
        // Advance level or handle win state here if needed
    }

    public static void ResetLevel()
    {
        var Hexes = HexParent.Instance().GetComponentsInChildren<Hex>();
        var tiles = TileParent.Instance().GetComponentsInChildren<Tile>();

        foreach (Hex hex in Hexes)
        {
            hex.SelfDestruct();
        }

        foreach (Tile tile in tiles)
        {
            tile.SelfDestruct();
        }
        
        hexes.Clear();
        tileDict.Clear();
    }
}
