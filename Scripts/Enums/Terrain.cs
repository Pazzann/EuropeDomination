using Godot;

namespace EuropeDominationDemo.Scripts.Enums;

public enum Terrain
{
    Mountains = 0,
    Plain = 1,
    Forest = 2,
    Field = 3,
    Coast = 4
}

public class TerrainColors
{
    public static Vector3[] Colors = new Vector3[5]
    {
        new Vector3(0.1f, 0.0f, 0.0f),
        new Vector3(0.2f, 0.2f, 0.6f),
        new Vector3(0.4f, 0.2f, 0.0f),
        new Vector3(0.4f, 0.8f, 0.0f),
        new Vector3(0.1f, 0.8f, 0.3f)
    };
}