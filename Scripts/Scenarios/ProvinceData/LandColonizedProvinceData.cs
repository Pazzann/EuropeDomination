using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
[Serializable]
public class LandColonizedProvinceData : LandProvinceData
{
    public List<Building> Buildings { get; set; }
    public int Development { get; set; }

    public TransportationRoute HarvestedTransport { get; set; }

    public Modifiers Modifiers { get; set; }
    public int Owner { get; set; }

    public double[] Resources { get; set; }
    public SpecialBuilding[] SpecialBuildings { get; set; }


    public LandColonizedProvinceData(
        int id,
        int owner,
        string name,
        int terrain,
        int good,
        int development,
        double[] resources,
        List<Building> buildings,
        Modifiers modifiers,
        SpecialBuilding[] specialBuildings,
        TransportationRoute harvestedTransport,
        int[] borderderingProvinces = null, 
        Vector2 centerOfWeight = new ()
    ) : base(id, name, terrain, good, borderderingProvinces, centerOfWeight)
    {
        Owner = owner;
        Development = development;

        Resources = resources;
        Buildings = buildings;

        Modifiers = modifiers;
        SpecialBuildings = specialBuildings;

        HarvestedTransport = harvestedTransport;
    }

    public float ProductionRate => Buildings.Where(d => d.IsFinished)
        .Aggregate(EngineState.MapInfo.Scenario.Settings.InitialProduction, (acc, x) => acc * x.Modifiers.ProductionEfficiency);

    public int UnlockedBuildingCount => EngineState.MapInfo.Scenario.Settings.DevForCommonBuilding.Where(a => a <= Development).ToArray().Length;
    public float TaxIncome => Development * EngineState.MapInfo.Scenario.Settings.TaxEarningPerDev;
    public float ManpowerGrowth => Development * EngineState.MapInfo.Scenario.Settings.ManpowerPerDev;

    public void SetRoute(TransportationRoute harvestedRoute)
    {
        HarvestedTransport = harvestedRoute;
    }
    
    [JsonConstructor]
    public LandColonizedProvinceData()
    {
        
    }
}