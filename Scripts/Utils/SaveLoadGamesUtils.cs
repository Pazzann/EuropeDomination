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
        File.WriteAllText(Path.Join(dirPath, "index.edsf"), SerializeScenario(scenario));
    }

    public static string SerializeScenario(Scenario scenario)
    {
        var stringified = "";
        stringified += "[Scenario]\n";
        stringified += "[Settings]\n";
        stringified += JsonSerializer.Serialize(scenario.Settings) + "\n";
        stringified += "[/Settings]\n";
        stringified += "[WastelandProvinceColors]\n";
        foreach (var pair in scenario.WastelandProvinceColors)
            stringified += "{" + pair.Key + "{" + $"X:{pair.Value.X},Y:{pair.Value.Y},Z:{pair.Value.Z}" + "}}"; 
        stringified += "\n";
        stringified += "[/WastelandProvinceColors]\n";
        stringified += "[WaterColor]\n{" + $"X:{scenario.WaterColor.X},Y:{scenario.WaterColor.Y},Z:{scenario.WaterColor.Z}" +"}\n[/WaterColor]\n";
        stringified += "[UncolonizedColor]\n{" + $"X:{scenario.UncolonizedColor.X},Y:{scenario.UncolonizedColor.Y},Z:{scenario.UncolonizedColor.Z}" +"}\n[/UncolonizedColor]\n";
        //stringified += "[Recipes]\n" + JsonSerializer.Serialize(scenario.Recipes) + "\n[/Recipes]\n";
        //fix recipes and other properies
        
        stringified += "[/Scenario]";
        return stringified;
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