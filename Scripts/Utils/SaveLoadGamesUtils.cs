using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
using EuropeDominationDemo.Scripts.Utils.JsonConverters;
using Godot;


namespace EuropeDominationDemo.Scripts.Utils;

public static class SaveLoadGamesUtils
{
    public static JsonSerializerOptions SerializerOptions => new()
    { 
        ReferenceHandler = ReferenceHandler.Preserve,
        WriteIndented = true,
        IncludeFields = true,
        PropertyNameCaseInsensitive = true,
        
        Converters =
        {
            new JsonGodotVector3Converter(),
            new JsonGodotVector2Converter(),
            new JsonSteamIdConverter(),
            new JsonRecipeConverter()
        }
    };
    
    public static string GameDocumentFiles {
        get
        {
            var myGames = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "My Games");
            if (!Directory.Exists(myGames))
                Directory.CreateDirectory(myGames);
            if(!Directory.Exists(Path.Combine(myGames, "Europe Domination")))
                Directory.CreateDirectory(Path.Combine(myGames, "Europe Domination"));
            return Path.Combine(myGames, "Europe Domination");
        }
    }
    public static string SavesPath => Path.Combine(GameDocumentFiles, "Save_Games");
    public static string ScenariosPath => Path.Combine(AppContext.BaseDirectory, "Scenarios");
    public static string TempPath => Path.Combine(GameDocumentFiles, ".temp");
    
    
    //loads scenario from zip
    public static void LoadScenario(string scenarioName, bool isSaveFile = false)
    {
        CleanCache();   
        
        var dirPath = Path.Join(isSaveFile ? SavesPath : ScenariosPath, scenarioName + ".zip");
        
        ZipFile.ExtractToDirectory(dirPath, TempPath);
        
        GlobalResources.MapTexture = Image.LoadFromFile(Path.Join(TempPath, "map.png"));
        GlobalResources.BuildingSpriteFrames = LoadSpriteFrames(Path.Join(TempPath, "Building"));
        GlobalResources.GoodSpriteFrames = LoadSpriteFrames(Path.Join(TempPath, "Goods"));
        GlobalResources.TechnologySpriteFrames = LoadSpriteFrames(Path.Join(TempPath, "Technology"));

        var scenario = JsonSerializer.Deserialize<CustomScenario>(File.ReadAllText(Path.Join(TempPath, "index.json")),
            SerializerOptions);
        EngineState.MapInfo = new MapData(scenario);
        
        CleanCache();
    }

    //cleans temp folder
    public static void CleanCache()
    {
        if (Directory.Exists(TempPath))
            Directory.Delete(TempPath, true);
        Directory.CreateDirectory(TempPath);
    }
    
    
    //you can run it exclusively  with rider
    public static void BuildScenario()
    {
        GlobalResources.MapTexture = GD.Load<CompressedTexture2D>("res://Sprites/EuropeMap.png").GetImage();
        GlobalResources.BuildingSpriteFrames = GD.Load<SpriteFrames>("res://Prefabs/SpriteFrames/Buildings.tres");
        GlobalResources.GoodSpriteFrames = GD.Load<SpriteFrames>("res://Prefabs/SpriteFrames/GoodSpriteFrames.tres");
        GlobalResources.TechnologySpriteFrames = GD.Load<SpriteFrames>("res://Prefabs/SpriteFrames/Technology.tres");
        Scenario scenario = new EuropeScenario();
        scenario.PlayerList = new Dictionary<ulong, int>();
        EngineState.MapInfo = new MapData(scenario);
        scenario.Settings.GameMode = GameModes.FullMapScenario;
        scenario.Settings.ResourceMode = ResourceModes.ScenarioSpawn;
        scenario.Init();
        SaveGame("Europe1700", scenario);
    }
    
    //checksums probably
    //saves scenario
    public static void SaveGame(string saveName, Scenario scenario)
    {
        
        CleanCache();
        if(!Directory.Exists(SavesPath))
            Directory.CreateDirectory(SavesPath);
        //creating save cache
        File.WriteAllText(Path.Join(TempPath, "index.json"), JsonSerializer.Serialize(scenario, SerializerOptions));
        GlobalResources.MapTexture.SavePng(Path.Join(TempPath, "map.png"));
        Directory.CreateDirectory(Path.Join(TempPath, "Goods"));
        Directory.CreateDirectory(Path.Join(TempPath, "Technology"));
        Directory.CreateDirectory(Path.Join(TempPath, "Building"));
        SaveSpriteFrames(Path.Join(TempPath, "Goods"), GlobalResources.GoodSpriteFrames);
        SaveSpriteFrames(Path.Join(TempPath, "Technology"), GlobalResources.TechnologySpriteFrames);
        SaveSpriteFrames(Path.Join(TempPath, "Building"), GlobalResources.BuildingSpriteFrames);
        //finishing
        
        //compressing it
        
        var dirPath = Path.Join(SavesPath, saveName + ".zip");
        if (File.Exists(dirPath))
            File.Delete(dirPath);
        
        ZipFile.CreateFromDirectory(TempPath, dirPath);
        //end compression
        
        CleanCache();
    }

    //saves sprite frames as a folder tree
    public static void SaveSpriteFrames(string savePath, SpriteFrames spriteFrames)
    {
        foreach (var animation in spriteFrames.GetAnimationNames())
        {
            var animPath = Path.Join(savePath, animation);
            Directory.CreateDirectory(animPath);
            for (int i = 0; i < spriteFrames.GetFrameCount(animation); i++)
            {
                spriteFrames.GetFrameTexture(animation, i).GetImage().SavePng(Path.Join(animPath, i + ".png"));
            }
        }
    }

    
    //load sprite frames from a folder tree
    public static SpriteFrames LoadSpriteFrames(string spriteFramesFolder)
    {
        var animations = Directory.GetDirectories(spriteFramesFolder);
        var spriteFrames = new SpriteFrames();
        spriteFrames.RemoveAnimation("default");
        foreach (var animation in animations)
        {
            var animName = Path.GetFileName(animation);
            spriteFrames.AddAnimation(animName);
            var frames = Directory.GetFiles(animation, "*.png");
            foreach (var frame in frames)
            {
                spriteFrames.AddFrame(animName, ImageTexture.CreateFromImage(Image.LoadFromFile(frame)));
            }
        }

        return spriteFrames;
    }
    
    public static string[] GetSavesList()
    {
        var dirPaths = Directory.GetFiles(SavesPath, "*.zip");
        var dirs = new string[dirPaths.Length];
        for (int i = 0; i < dirPaths.Length; i++)
        {
            dirs[i] = Path.GetFileName(dirPaths[i]).Split(".zip")[0];
        }

        return dirs;
    }

    public static string[] GetScenariosList()
    {
        var dirPaths = Directory.GetFiles(ScenariosPath, "*.zip");
        var dirs = new string[dirPaths.Length];
        for (int i = 0; i < dirPaths.Length; i++)
        {
            dirs[i] = Path.GetFileName(dirPaths[i]).Split(".zip")[0];
        }

        return dirs;
    }
}