using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class Terrain
{
    public Vector3 Color;
    public int Id;
    public Modifiers Modifiers;
    public string Name;

    public Terrain(string name, int id, Vector3 color, Modifiers modifiers)
    {
        Name = name;
        Id = id;
        Color = color;
        Modifiers = modifiers;
    }
}