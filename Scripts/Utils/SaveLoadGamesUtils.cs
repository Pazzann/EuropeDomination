using System;
using System.IO;

namespace EuropeDominationDemo.Scripts.Utils;

public static class SaveLoadGamesUtils
{
    public static string SavesPath => Path.Combine(AppContext.BaseDirectory, "Save_Games");
    
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
}