using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
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
    
    public Modifiers TotalModifiers{
        get
        {
            Modifiers totalModifiers = Modifiers.DefaultModifiers();
            totalModifiers += Buildings.Aggregate(Modifiers.DefaultModifiers(), (acc, x) => acc + x.Modifiers);
            totalModifiers += EngineState.MapInfo.Scenario.Countries[Owner].TotalModifiers;
            return totalModifiers;
        }
        
    }

    public void ConstructionDayTick()
    {
        foreach (var building in Buildings.Where(building => !building.IsFinished))
            building.DayTick();
        
        foreach (var specialBuilding in SpecialBuildings.Where(specialBuilding => specialBuilding != null))
            specialBuilding.DayTick();
    }
    
    
    public void Produce()
    {
        Resources[Good] += ProductionRate;
    }
    
    public void Transport()
    {
        if( HarvestedTransport != null )
            HarvestedTransport.TransportOnce(TotalModifiers);
    }
    
    public void SpecialBuildingProduce()
    {
        foreach (Factory factory in SpecialBuildings.Where(a => a is Factory b && b.Recipe != -1))
        {
            if (EngineState.MapInfo.Scenario.Recipes[factory.Recipe].Ingredients.Where((ingredient) =>
                        Resources[ingredient.Key] - ingredient.Value * factory.ProductionRate < 0).ToArray()
                    .Length <= 0)
            {
                foreach (var ingredient in EngineState.MapInfo.Scenario.Recipes[factory.Recipe].Ingredients)
                    Resources[ingredient.Key] -= ingredient.Value * factory.ProductionRate;

                Resources[EngineState.MapInfo.Scenario.Recipes[factory.Recipe].Output] +=
                    EngineState.MapInfo.Scenario.Recipes[factory.Recipe].OutputAmount * factory.ProductionRate;
                factory.ProductionRate = Mathf.Min(factory.ProductionRate + factory.ProductionGrowthRate,
                    factory.MaxProductionRate);
            }
            else
            {
                factory.ProductionRate = Mathf.Max(factory.ProductionRate - factory.ProductionGrowthRate, 0.1f);
            }
        }
    }
    
    public void SpecialBuildingTransport()
    {
        foreach (var specialBuilding in SpecialBuildings)
        {
            if (specialBuilding is Factory { TransportationRoute: not null } factory)
                factory.TransportationRoute.TransportOnce(TotalModifiers);
            if (specialBuilding is StockAndTrade stockAndTrade)
                stockAndTrade.Transport(TotalModifiers);
            if (specialBuilding is Dockyard dockyard)
                dockyard.Transport(TotalModifiers);
            
        }
            
    }
    
    public float ProductionRate => (EngineState.MapInfo.Scenario.Settings.InitialProductionPerDev * Development + TotalModifiers.ProductionBonus) * TotalModifiers.ProductionEfficiency;
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