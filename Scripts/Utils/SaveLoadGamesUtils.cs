using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Xml.Serialization;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;

namespace EuropeDominationDemo.Scripts.Utils;

public static class SaveLoadGamesUtils
{
    public static string SavesPath => Path.Combine(AppContext.BaseDirectory, "Save_Games");
    public static string ScenariosPath => Path.Combine(AppContext.BaseDirectory, "Save_Games");
    
    public static string[] GetSavesList()
    {
        var dirPaths = Directory.GetDirectories(SavesPath);
        var dirs = new string[dirPaths.Length];
        for (int i = 0; i < dirPaths.Length; i++)
        {
            dirs[i] = Path.GetFileName(dirPaths[i]);
        }
        return dirs;
    }

    public static void SaveGame(string saveName, Scenario scenario)
    {
        var dirPath = Path.Join(SavesPath, saveName);
        Directory.CreateDirectory(dirPath);
        string scenarioJson = JsonSerializer.Serialize(scenario);
        File.WriteAllText(Path.Join(dirPath, "index.json"), scenarioJson);
    }

    public static string SerializeScenario(Scenario scenario)
    {
        Type scenarioType =  scenario.GetType();

        return "";
    }

    public static string[] GetScenariosList()
    {
        var dirPaths = Directory.GetDirectories(ScenariosPath);
        var dirs = new string[dirPaths.Length];
        for (int i = 0; i < dirPaths.Length; i++)
        {
            dirs[i] = Path.GetFileName(dirPaths[i]);
        }
        return dirs;
    }

    public static void LoadAllSpriteFrames(string savePath)
    {
        
        
        //return new SpriteFrames();
    }
}