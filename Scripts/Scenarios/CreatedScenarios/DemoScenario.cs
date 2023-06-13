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
            { "Great Britian", 0 },
            { "France", 1 },
            { "Sweden", 2 }
        };
        CountriesColors = new Vector3[]
        {
            new Vector3(0.0f, 1.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 1.0f),
            new Vector3(1.0f, 0.0f, 0.0f),
        };
        CountriesNames = new string[]
        {
            "Great Britian",
            "France",
            "Sweden"
        };
        CountriesData = new CountryData[]
        {
            new CountryData(),
            new CountryData(),
            new CountryData()
        };
        Map = new ProvinceData[14]
        {
            new ProvinceData(0, Countries["Great Britian"], "Rekyavik", Terrain.Coast, Good.Iron),
            new ProvinceData(1,  Countries["Great Britian"], "Rekyavik", Terrain.Field, Good.Iron),
            new ProvinceData(2,  Countries["Great Britian"], "Rekyavik", Terrain.Field, Good.Iron),
            new ProvinceData(3,  Countries["Great Britian"], "Rekyavik", Terrain.Forest, Good.Iron),
            new ProvinceData(4,  Countries["France"], "Rekyavik", Terrain.Plain, Good.Iron),
            new ProvinceData(5,  Countries["France"], "Rekyavik", Terrain.Plain, Good.Iron),
            new ProvinceData(6,  Countries["France"], "Rekyavik", Terrain.Plain, Good.Iron),
            new ProvinceData(7,  Countries["France"], "Rekyavik", Terrain.Plain, Good.Wheat),
            new ProvinceData(8,  Countries["France"], "Rekyavik", Terrain.Plain, Good.Wheat),
            new ProvinceData(9,  Countries["Sweden"], "Rekyavik", Terrain.Mountains, Good.Wheat),
            new ProvinceData(10, Countries["Sweden"], "Rekyavik", Terrain.Mountains, Good.Wheat),
            new ProvinceData(11, Countries["Sweden"], "Rekyavik", Terrain.Mountains, Good.Wheat),
            new ProvinceData(12,  Countries["Sweden"], "Rekyavik", Terrain.Coast, Good.Wheat),
            new ProvinceData(13, Countries["Sweden"], "Rekyavik", Terrain.Coast, Good.Wheat),
        };
        Map = GameMath.CalculateBorderProvinces(Map, mapTexture);
        var centers = GameMath.CalculateCenterOfProvinceWeight(mapTexture, ProvinceCount);
        for (int i = 0; i < ProvinceCount; i++)
        {
            Map[i].CenterOfWeight = centers[i];
        }
    }
}