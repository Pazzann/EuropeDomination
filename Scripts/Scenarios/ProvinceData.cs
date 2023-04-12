using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class ProvinceData
{
    public readonly int Id;
    public readonly string Name;
    public int Owner;
    public Vector2 CenterOfWeight;
    public int[] borderProvinces;

    public ProvinceData(int id, int countryId, string name)
    {
        this.Id = id;
        this.Owner = countryId;
        this.Name = name;
        this.borderProvinces = new int[] { };
    }
}