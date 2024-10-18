using System;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Technology;

[Serializable]
public class Technology
{
    public Technology(string technologyName,Modifiers modifiers, int initialCost, int researchTime, double[] resourcesRequired, int buildingToUnlock = -1, int recipyToUnlock = -1)
    {
        TechnologyName = technologyName;
        Modifiers = modifiers;
        InitialCost = initialCost;
        ResearchTime = researchTime;
        ResourcesRequired = resourcesRequired;
        BuildingToUnlock = buildingToUnlock;
        RecipyToUnlock = recipyToUnlock;
    }

    public bool CheckIfMeetsRequirements(int countryId)
    {
        var countryData = EngineState.MapInfo.Scenario.Countries[countryId];
        var capital = EngineState.MapInfo.Scenario.Map[countryData.CapitalId] as LandColonizedProvinceData;
        return Good.CheckIfMeetsRequirements(capital.Resources, ResourcesRequired) && countryData.Money - InitialCost > -EngineVariables.Eps;
    }

    public void ReduceByRequirments(int countryId)
    {
        var countryData = EngineState.MapInfo.Scenario.Countries[countryId];
        var capital = EngineState.MapInfo.Scenario.Map[countryData.CapitalId] as LandColonizedProvinceData;
        countryData.Money -= InitialCost;
        capital.Resources = Good.DecreaseGoodsByGoods(capital.Resources, ResourcesRequired);
        return;
    }

    public string TechnologyName { get; }
    public Modifiers Modifiers { get; }
    public int InitialCost { get; }
    public int ResearchTime { get; }
    public double[] ResourcesRequired { get; }
    public int BuildingToUnlock { get; }
    public int RecipyToUnlock { get; }
}