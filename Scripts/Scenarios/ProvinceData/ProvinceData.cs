using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public abstract class ProvinceData
{
    public readonly int Id;
    public Vector2 CenterOfWeight;
    public int[] BorderderingProvinces;
    public string Name;

    protected ProvinceData(int id, string name)
    {
        Id = id;
        Name = name;
    }
}