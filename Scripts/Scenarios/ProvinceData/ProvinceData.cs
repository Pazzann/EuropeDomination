using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public abstract class ProvinceData
{
    public int Id { get; }
    public int[] BorderderingProvinces { get; set; }
    public Vector2 CenterOfWeight { get; set; }
    public string Name { get; set; }

    protected ProvinceData(int id, string name)
    {
        Id = id;
        Name = name;
    }
}