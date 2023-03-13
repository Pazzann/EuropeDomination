using Godot;

namespace EuropeDominationDemo.Scripts.Enums;

public enum Resources
{
    Iron = 0,
    Wheat = 1,
}

public class ResourcesColors
{
    public static Vector3[] Colors = new Vector3[2]
    {
        new Vector3(0.5f, 0.3f, 0.0f),
        new Vector3(0.7f, 0.8f, 0.0f)
    };
}