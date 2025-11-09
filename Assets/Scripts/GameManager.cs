using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class GameManager
{
    public static int level = 0;
    public static float unitDuration = 0.1f;
    public static bool isPaused = true;
    public static Dictionary<Vector2Int, Tile> tileDict = new();
    public static List<Hex> hexes = new();
    public static List<Other> others = new();
    public static int numberOfMoves;
    public static bool scrollFlag = false;

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
        ResumeGame();
        int levelToStart = level;
        if (levelToStart == 0) levelToStart = 1;
        LevelGenerator.Instance().GenerateLevel(levelToStart);
    }

    public static void LevelWon()
    {
        if (level == 0) level = 2;
        else level++;
        scrollFlag = true;
        // Trigger level won event - inventory increment and other handlers will respond
        EventManager.Instance().OnLevelWon();
        ScreenManager.Instance().ShowScreen(ScreenType.WinScreen);
        PauseGame();
        ResetLevel();
    }

    public static void LevelLost()
    {
        if(level == 0) level = 1;
        ScreenManager.Instance().ShowScreen(ScreenType.LoseScreen);
        PauseGame();
        ResetLevel();
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

    public static void PauseGame()
    {
        isPaused = true;
        LevelManager.Instance().HandleMoveText();
    }

    public static void ResumeGame()
    {
        isPaused = false;
        LevelManager.Instance().HandleMoveText();
    }
}
