using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class Terrain
{
    public string Name;
    public int Id;
    public Vector3 Color;
    public Modifiers Modifiers;

    public Terrain(string name, int id, Vector3 color, Modifiers modifiers)
    {
        Name = name;
        Id = id;
        Color = color;
        Modifiers = modifiers;
    }

}