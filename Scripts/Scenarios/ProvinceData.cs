using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class ProvinceData
{
    public readonly int Id;
    public readonly string Name;
    public Vector3 Color;

    public ProvinceData(int id, Vector3[] countryColors, int countryId, string name)
    {
        this.Id = id;
        this.Color = countryColors[countryId];
        this.Name = name;
    }
}