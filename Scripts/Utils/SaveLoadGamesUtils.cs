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

/// <summary>
/// Utility class for saving and loading game data.
/// </summary>
public static class SaveLoadGamesUtils
{
    /// <summary>
    /// Gets the JsonSerializerOptions with predefined settings for serialization.
    /// </summary>
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

    /// <summary>
    /// Gets the file path for the game documents directory.
    /// This directory is located in the user's "My Documents" folder under "My Games\Europe Domination".
    /// If the directory does not exist, it will be created.
    /// </summary>
    /// <value>
    /// The file path for the game documents directory.
    /// </value>
    public static string GameDocumentFiles
    {
        get
        {
            var myGames = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                "My Games");
            if (!Directory.Exists(myGames))
                Directory.CreateDirectory(myGames);
            if (!Directory.Exists(Path.Combine(myGames, "Europe Domination")))
                Directory.CreateDirectory(Path.Combine(myGames, "Europe Domination"));
            return Path.Combine(myGames, "Europe Domination");
        }
    }

    /// <summary>
    /// Gets the file path for the save games directory.
    /// </summary>
    public static string SavesPath => Path.Combine(GameDocumentFiles, "Save_Games");

    /// <summary>
    /// Gets the file path for the scenarios directory.
    /// </summary>
    public static string ScenariosPath => Path.Combine(AppContext.BaseDirectory, "Scenarios");

    /// <summary>
    /// Gets the file path for the temporary directory used for caching.
    /// </summary>
    public static string TempPath => Path.Combine(GameDocumentFiles, ".temp");


    /// <summary>
    /// Loads a scenario from a specified file.
    /// </summary>
    /// <param name="scenarioName">The name of the scenario to load.</param>
    /// <param name="isSaveFile">Indicates whether the scenario is a save file.</param>
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


    /// <summary>
    /// Cleans the temporary cache directory by deleting it and then recreating it.
    /// </summary>
    public static void CleanCache()
    {
        if (Directory.Exists(TempPath))
            Directory.Delete(TempPath, true);
        Directory.CreateDirectory(TempPath);
    }


    /// <summary>
    /// Builds a scenario and saves it to a file.
    /// </summary>
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

    /// <summary>
    /// Saves the current game state to a file.
    /// </summary>
    /// <param name="saveName"></param>
    /// <param name="scenario"></param>
    public static void SaveGame(string saveName, Scenario scenario)
    {
        CleanCache();
        if (!Directory.Exists(SavesPath))
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

    /// <summary>
    /// Saves the sprite frames to a specified directory.
    /// </summary>
    /// <param name="savePath"></param>
    /// <param name="spriteFrames"></param>
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


    /// <summary>
    /// Loads the sprite frames from a specified directory.
    /// </summary>
    /// <param name="spriteFramesFolder"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Loads a save game from a specified file.
    /// </summary>
    /// <returns>
    /// The list of saved games available for loading.
    /// </returns>
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

    /// <summary>
    /// Gets the list of scenarios available for loading.
    /// </summary>
    /// <returns>
    /// The list of scenarios available for loading.
    /// </returns>
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