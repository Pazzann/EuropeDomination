using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public abstract class ProvinceData
{
    public readonly int Id;
    public int[] BorderderingProvinces;
    public Vector2 CenterOfWeight;
    public string Name;

    protected ProvinceData(int id, string name)
    {
        Id = id;
        Name = name;
    }
}