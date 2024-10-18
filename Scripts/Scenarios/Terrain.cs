using System;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

[Serializable]
public class Terrain
{
    public Vector3 Color { get; set; }
    public int Id { get; set; }
    public Modifiers Modifiers { get; set; }
    public string Name { get; set; }

    public Terrain(string name, int id, Vector3 color, Modifiers modifiers)
    {
        Name = name;
        Id = id;
        Color = color;
        Modifiers = modifiers;
    }
}