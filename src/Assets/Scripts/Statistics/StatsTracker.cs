using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StatsTracker
{
    private static float _timeInGame = 0.0f;
    public static float TimeInGame { get { return _timeInGame; } set { _timeInGame = value; } }

    private static float _timeInMenus = 0.0f;
    public static float TimeInMenus { get { return _timeInMenus; } set { _timeInMenus = value; } }

    private static int _totalMoves = 0;
    public static int TotalMoves { get { return _totalMoves; } set { _totalMoves = value; } }

    private static int _totalScore = 0;
    public static int TotalScore { get { return _totalScore; } set { _totalScore = value; } }

    private static int _grassTilesMowed = 0;
    public static int GrassTilesMowed { get { return _grassTilesMowed; } set { _grassTilesMowed = value; } }

    private static int _levelsCompleted;
    public static int LevelsCompleted { get { return _levelsCompleted; } set { _levelsCompleted = value; } }

    private static int _levelsReplayed;
    public static int LevelsReplayed { get { return _levelsReplayed; } set { _levelsReplayed = value; } }

    private static int _scoresImproved;
    public static int ScoresImproved { get { return _scoresImproved; } set { _scoresImproved = value; } }

    public enum StatTypes
    {
        TimeInGame,
        TimeInMenus,
        TotalMoves,
        TotalScore,
        GrassTilesMowed,
        LevelsCompleted,
        LevelsReplayed,
        ScoresImproved,
    }

    public static Dictionary<StatTypes, string> StatNames = new Dictionary<StatTypes, string>()
    {
        { StatTypes.TimeInGame, "TimeInGame" },
        { StatTypes.TimeInMenus, "TimeInMenus" },
        { StatTypes.TotalMoves, "TotalMoves" },
        { StatTypes.TotalScore, "TotalScore" },
        { StatTypes.GrassTilesMowed, "GrassTilesMowed" },
        { StatTypes.LevelsCompleted, "LevelsCompleted" },
        { StatTypes.LevelsReplayed, "LevelsReplayed" },
        { StatTypes.ScoresImproved, "ScoresImproved" },
    };

    private static void Save()
    {
        PlayerPrefs.Save();
    }

    public static void LoadGameStats()
    {
        TimeInGame = PlayerPrefs.GetFloat(StatNames[StatTypes.TimeInGame]);
        TimeInMenus = PlayerPrefs.GetFloat(StatNames[StatTypes.TimeInMenus]);
        TotalMoves = PlayerPrefs.GetInt(StatNames[StatTypes.TotalMoves]);
        TotalScore = PlayerPrefs.GetInt(StatNames[StatTypes.TotalScore]);
        GrassTilesMowed = PlayerPrefs.GetInt(StatNames[StatTypes.GrassTilesMowed]);
        LevelsCompleted = PlayerPrefs.GetInt(StatNames[StatTypes.LevelsCompleted]);
        LevelsReplayed = PlayerPrefs.GetInt(StatNames[StatTypes.LevelsReplayed]);
        ScoresImproved = PlayerPrefs.GetInt(StatNames[StatTypes.ScoresImproved]);
    }

    public static void SaveGameStats()
    {
        PlayerPrefs.SetFloat(StatNames[StatTypes.TimeInGame], TimeInGame);
        PlayerPrefs.SetFloat(StatNames[StatTypes.TimeInMenus], TimeInMenus);
        PlayerPrefs.SetInt(StatNames[StatTypes.TotalMoves], TotalMoves);
        PlayerPrefs.SetInt(StatNames[StatTypes.TotalScore], TotalScore);
        PlayerPrefs.SetInt(StatNames[StatTypes.GrassTilesMowed], GrassTilesMowed);
        PlayerPrefs.SetInt(StatNames[StatTypes.LevelsCompleted], LevelsCompleted);
        PlayerPrefs.SetInt(StatNames[StatTypes.LevelsReplayed], LevelsReplayed);
        PlayerPrefs.SetInt(StatNames[StatTypes.ScoresImproved], ScoresImproved);
    }

}
