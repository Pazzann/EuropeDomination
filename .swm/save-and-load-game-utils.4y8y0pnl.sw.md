---
title: Save and Load Game Utils
---
# Introduction

This document will walk you through the implementation of the <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="20:6:6" line-data="public static class SaveLoadGamesUtils">`SaveLoadGamesUtils`</SwmToken> class.

The class provides utility methods for saving and loading game scenarios, handling file paths, and managing temporary directories.

We will cover:

1. Why we need custom serializer options.
2. How file paths are managed.
3. How scenarios are loaded.
4. How scenarios are saved.
5. How sprite frames are handled.
6. How to list saved games and scenarios.

# Custom serializer options

<SwmSnippet path="/Scripts/Utils/SaveLoadGamesUtils.cs" line="25">

---

The <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="25:7:7" line-data="    public static JsonSerializerOptions SerializerOptions =&gt; new()">`SerializerOptions`</SwmToken> property defines custom JSON serialization settings. These settings ensure that complex objects like vectors and IDs are correctly serialized and deserialized.

```
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
```

---

</SwmSnippet>

# File paths management

<SwmSnippet path="/Scripts/Utils/SaveLoadGamesUtils.cs" line="49">

---

The class defines several properties to manage file paths for game documents, save files, scenarios, and temporary files. This ensures that all file operations are centralized and consistent.

```
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

```

---

</SwmSnippet>

# Loading scenarios

<SwmSnippet path="/Scripts/Utils/SaveLoadGamesUtils.cs" line="84">

---

The <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="84:7:7" line-data="    public static void LoadScenario(string scenarioName, bool isSaveFile = false)">`LoadScenario`</SwmToken> method extracts a scenario from a zip file, loads various resources, and updates the game state. This method is essential for initializing the game with the correct data.

```
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
```

---

</SwmSnippet>

# Cleaning the cache

<SwmSnippet path="/Scripts/Utils/SaveLoadGamesUtils.cs" line="108">

---

The <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="108:7:7" line-data="    public static void CleanCache()">`CleanCache`</SwmToken> method ensures that the temporary directory is clean before and after loading or saving scenarios. This prevents leftover files from interfering with new operations.

```
    public static void CleanCache()
    {
        if (Directory.Exists(TempPath))
            Directory.Delete(TempPath, true);
        Directory.CreateDirectory(TempPath);
    }
```

---

</SwmSnippet>

# Building scenarios

<SwmSnippet path="/Scripts/Utils/SaveLoadGamesUtils.cs" line="119">

---

The <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="119:7:7" line-data="    public static void BuildScenario()">`BuildScenario`</SwmToken> method initializes a new scenario, sets up game resources, and saves the scenario. This method is useful for creating new game scenarios programmatically.

```
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

```

---

</SwmSnippet>

# Saving scenarios

<SwmSnippet path="Scripts/Utils/SaveLoadGamesUtils.cs" line="139">

---

The <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="131:1:1" line-data="        SaveGame(&quot;Europe1700&quot;, scenario);">`SaveGame`</SwmToken> method serializes the current game state, saves various resources, and compresses the data into a zip file. This method is crucial for preserving the game state between sessions.

```
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
```

---

</SwmSnippet>

# Saving sprite frames

<SwmSnippet path="/Scripts/Utils/SaveLoadGamesUtils.cs" line="172">

---

The <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="172:7:7" line-data="    public static void SaveSpriteFrames(string savePath, SpriteFrames spriteFrames)">`SaveSpriteFrames`</SwmToken> method saves sprite frames as individual PNG files in a directory structure. This allows for easy storage and retrieval of sprite animations.

```
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
```

---

</SwmSnippet>

# Loading sprite frames

<SwmSnippet path="/Scripts/Utils/SaveLoadGamesUtils.cs" line="191">

---

The <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="191:7:7" line-data="    public static SpriteFrames LoadSpriteFrames(string spriteFramesFolder)">`LoadSpriteFrames`</SwmToken> method loads sprite frames from a directory structure. This method is used to reconstruct sprite animations from saved files.

```
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
```

---

</SwmSnippet>

# Listing saved games and scenarios

<SwmSnippet path="/Scripts/Utils/SaveLoadGamesUtils.cs" line="216">

---

The <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="216:9:9" line-data="    public static string[] GetSavesList()">`GetSavesList`</SwmToken> and <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="234:9:9" line-data="    public static string[] GetScenariosList()">`GetScenariosList`</SwmToken> methods return lists of saved games and scenarios, respectively. These methods are useful for displaying available options to the user.

```
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
```

---

</SwmSnippet>

# How to use this class

1. **Initialize the class**: No initialization is needed as all methods and properties are static.
2. **Load a scenario**: Call <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="84:7:7" line-data="    public static void LoadScenario(string scenarioName, bool isSaveFile = false)">`LoadScenario`</SwmToken>`("ScenarioName")` to load a scenario.
3. **Save a game**: Call <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="139:7:7" line-data="    public static void SaveGame(string saveName, Scenario scenario)">`SaveGame`</SwmToken>`("SaveName", `<SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="131:8:8" line-data="        SaveGame(&quot;Europe1700&quot;, scenario);">`scenario`</SwmToken>`)` to save the current game state.
4. **Build a scenario**: Call <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="119:7:9" line-data="    public static void BuildScenario()">`BuildScenario()`</SwmToken> to create and save a new scenario.
5. **List saves and scenarios**: Use <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="216:9:11" line-data="    public static string[] GetSavesList()">`GetSavesList()`</SwmToken> and <SwmToken path="/Scripts/Utils/SaveLoadGamesUtils.cs" pos="234:9:11" line-data="    public static string[] GetScenariosList()">`GetScenariosList()`</SwmToken> to get available saves and scenarios.

This class centralizes all file operations related to saving and loading game data, ensuring consistency and reliability.

<SwmMeta version="3.0.0" repo-id="Z2l0aHViJTNBJTNBRXVyb3BlRG9taW5hdGlvbkRlbW8lM0ElM0FQYXp6YW5u" repo-name="EuropeDominationDemo"><sup>Powered by [Swimm](https://app.swimm.io/)</sup></SwmMeta>
