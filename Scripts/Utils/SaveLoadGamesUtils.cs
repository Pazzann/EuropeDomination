using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Utils.JsonConverters;

namespace EuropeDominationDemo.Scripts.Utils;

public static class SaveLoadGamesUtils
{
    public static JsonSerializerOptions SerializerOptions =>new()
    {
        ReferenceHandler = ReferenceHandler.Preserve,
        WriteIndented = true,
        Converters =
        {
            new JsonGodotVector3Converter(),
            new JsonGodotVector2Converter(),
            new JsonSteamIdConverter()
        }
    };
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

    public static void LoadGame(string saveName)
    {
        var dirPath = Path.Join(SavesPath, saveName);
        
        //using FileStream fs = new FileStream(Path.Join(dirPath, "index.dat"), FileMode.OpenOrCreate);
        //BinaryFormatter b = new BinaryFormatter();
        //var a =  (Scenario)b.Deserialize(fs);
        //GD.Print(a);
    }
    public static void LoadScenario(string saveName)
    {
        var dirPath = Path.Join(SavesPath, saveName);
        
        //using FileStream fs = new FileStream(Path.Join(dirPath, "index.dat"), FileMode.OpenOrCreate);
        //BinaryFormatter b = new BinaryFormatter();
        //var a =  (Scenario)b.Deserialize(fs);
        //GD.Print(a);
    }


    public static void SaveGame(string saveName, Scenario scenario)
    {
        var dirPath = Path.Join(SavesPath, saveName);
        Directory.CreateDirectory(dirPath);
        File.WriteAllText(Path.Join(dirPath, "index.json"), JsonSerializer.Serialize(scenario, SerializerOptions));
        
        //todo save sprite frames
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