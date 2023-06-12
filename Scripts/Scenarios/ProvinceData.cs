using EuropeDominationDemo.Scripts.Enums;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class ProvinceData
{
    public readonly int Id;
    public readonly string Name;
    public int Owner;
    public Vector2 CenterOfWeight;
    public int[] BorderProvinces;
    
    public readonly Terrain Terrain;
    public readonly Good Good;

    public ProvinceData(int id, int countryId, string name, Terrain terrain, Good good)
    {
        this.Id = id;
        this.Owner = countryId;
        this.Name = name;
        this.Terrain = terrain;
        
        this.BorderProvinces = new int[] { };
        this.Good = good;
    }
}