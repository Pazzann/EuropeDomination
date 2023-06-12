using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Math;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;

public class DemoScenario : Scenario
{
    public override Dictionary<string, int> Countries { get; }
    public override Vector3[] CountriesColors { get; }
    public override string[] CountriesNames { get; }
    public override int ProvinceCount { get; }
    public override CountryData[] CountriesData { get; set; }
    public override ProvinceData[] Map { get; set; }
    public DemoScenario(Image mapTexture)
    {
        ProvinceCount = 14;
        Countries = new Dictionary<string, int>()
        {
            { "Green", 0 },
            { "Blue", 1 },
            { "Red", 2 }
        };
        CountriesColors = new Vector3[]
        {
            new Vector3(0.0f, 1.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 1.0f),
            new Vector3(1.0f, 0.0f, 0.0f),
        };
        CountriesNames = new string[]
        {
            "Green",
            "Blue",
            "Red"
        };
        CountriesData = new CountryData[]
        {
            new CountryData(),
            new CountryData(),
            new CountryData()
        };
        Map = new ProvinceData[14]
        {
            new ProvinceData(0, Countries["Blue"], "Rekyavik", Terrain.Coast, Good.Iron),
            new ProvinceData(1,  Countries["Blue"], "Rekyavik", Terrain.Field, Good.Iron),
            new ProvinceData(2,  Countries["Blue"], "Rekyavik", Terrain.Field, Good.Iron),
            new ProvinceData(3,  Countries["Blue"], "Rekyavik", Terrain.Forest, Good.Iron),
            new ProvinceData(4,  Countries["Red"], "Rekyavik", Terrain.Plain, Good.Iron),
            new ProvinceData(5,  Countries["Red"], "Rekyavik", Terrain.Plain, Good.Iron),
            new ProvinceData(6,  Countries["Red"], "Rekyavik", Terrain.Plain, Good.Iron),
            new ProvinceData(7,  Countries["Red"], "Rekyavik", Terrain.Plain, Good.Wheat),
            new ProvinceData(8,  Countries["Red"], "Rekyavik", Terrain.Plain, Good.Wheat),
            new ProvinceData(9,  Countries["Green"], "Rekyavik", Terrain.Mountains, Good.Wheat),
            new ProvinceData(10, Countries["Green"], "Rekyavik", Terrain.Mountains, Good.Wheat),
            new ProvinceData(11, Countries["Green"], "Rekyavik", Terrain.Mountains, Good.Wheat),
            new ProvinceData(12,  Countries["Green"], "Rekyavik", Terrain.Coast, Good.Wheat),
            new ProvinceData(13, Countries["Green"], "Rekyavik", Terrain.Coast, Good.Wheat),
        };
        Map = GameMath.CalculateBorderProvinces(Map, mapTexture);
        var centers = GameMath.CalculateCenterOfProvinceWeight(mapTexture, ProvinceCount);
        for (int i = 0; i < ProvinceCount; i++)
        {
            Map[i].CenterOfWeight = centers[i];
        }
    }
}