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
    public int Development;

    public float[] Resources;
    
    public readonly Terrain Terrain;
    public readonly Good Good;

    public ProvinceData(int id, int countryId, string name, Terrain terrain, Good good, int development, float[] resources)
    {
        this.Id = id;
        this.Owner = countryId;
        this.Name = name;
        this.Terrain = terrain;
        this.Development = development;

        this.Resources = resources;
        
        this.BorderProvinces = new int[] { };
        this.Good = good;
    }
}