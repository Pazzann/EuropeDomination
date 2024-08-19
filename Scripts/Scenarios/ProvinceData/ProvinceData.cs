using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public abstract class ProvinceData
{
    public readonly int Id;
    public Vector2 CenterOfWeight;
    public int[] BorderderingProvinces;

    protected ProvinceData(int id)
    {
        Id = id;
    }
}