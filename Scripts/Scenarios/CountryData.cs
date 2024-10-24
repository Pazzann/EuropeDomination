using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;
using EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

[Serializable]
public class CountryData
{
    public List<Admiral> Admirals { get; set; }
    public int CapitalId { get; set; }
    public Vector3 Color { get; set; }

    public Dictionary<int, List<DiplomacyAgreement>> DiplomacyAgreements { get; set; }

    public float Money { get; set; }
    public int Manpower { get; set; }
    public Modifiers Modifiers { get; set; }
    public string Name { get; set; }

    public List<General> Generals { get; set; }
    public List<Template> RegimentTemplates { get; set; }
    public List<UnitData> Units { get; set; }

    public List<List<List<bool>>> ResearchedTechnologies { get; set; }
    public Dictionary<Vector3I, int> CurrentlyResearching { get; init; }


    public Modifiers TotalModifiers
    {
        get
        {
            var a = Modifiers.DefaultModifiers();
            a += Modifiers;
            a += TechnologyModifiers;
            a += NationalIdeas;
            a += ConsumableGoodsModifiers;
            return a;
        }
    }

    public Modifiers TechnologyModifiers
    {
        get
        {
            var a = Modifiers.DefaultModifiers();
            for (int i = 0; i < EngineState.MapInfo.Scenario.TechnologyTrees.Length; i++)
            {
                var tree = EngineState.MapInfo.Scenario.TechnologyTrees[i];
                for (int j = 0; j < tree.TechnologyLevels.Count; j++)
                {
                    var level = tree.TechnologyLevels[j];
                    for (int k = 0; k < level.Technologies.Count; k++)
                    {
                        var technology = level.Technologies[k];
                        if (ResearchedTechnologies[i][j][k])
                            a += technology.Modifiers;
                    }
                }
            }

            return a;
        }
    }

    public List<int> UnlockedRecipies
    {
        get
        {
            var a = new List<int>();
            for (int i = 0; i < EngineState.MapInfo.Scenario.TechnologyTrees.Length; i++)
            {
                var tree = EngineState.MapInfo.Scenario.TechnologyTrees[i];
                for (int j = 0; j < tree.TechnologyLevels.Count; j++)
                {
                    var level = tree.TechnologyLevels[j];
                    for (int k = 0; k < level.Technologies.Count; k++)
                    {
                        var technology = level.Technologies[k];
                        if (technology.RecipyToUnlock > -1 && ResearchedTechnologies[i][j][k])
                            a.Add(technology.RecipyToUnlock);
                    }
                }
            }

            return a;
        }
    }

    public List<int> UnlockedBuildings
    {
        get
        {
            var a = new List<int>();
            for (int i = 0; i < EngineState.MapInfo.Scenario.TechnologyTrees.Length; i++)
            {
                var tree = EngineState.MapInfo.Scenario.TechnologyTrees[i];
                for (int j = 0; j < tree.TechnologyLevels.Count; j++)
                {
                    var level = tree.TechnologyLevels[j];
                    for (int k = 0; k < level.Technologies.Count; k++)
                    {
                        var technology = level.Technologies[k];
                        if (technology.BuildingToUnlock > -1 && ResearchedTechnologies[i][j][k])
                            a.Add(technology.BuildingToUnlock);
                    }
                }
            }

            return a;
        }
    }

    public LandColonizedProvinceData Capital =>
        EngineState.MapInfo.Scenario.Map[CapitalId] as LandColonizedProvinceData;

    public Modifiers NationalIdeas { get; init; }

    //bool checks if the requirments for the next month are fullfilled
    public Dictionary<int, bool> ConsumableGoods { get; init; }

    public int Id { get; init; }

    public CountryData(int id, string name, Vector3 color, Modifiers modifiers, int money, int manpower,
        List<General> generals, List<Admiral> admirals, List<UnitData> units, List<Template> templates,
        Dictionary<int, List<DiplomacyAgreement>> diplomacyAgreements, int capitalId,
        Dictionary<Vector3I, int> currentlyResearching, List<List<List<bool>>> researchedTechnologies,
        Modifiers nationalIdeas, Dictionary<int, bool> consumableGoods)
    {
        Id = id;
        Name = name;
        Color = color;
        Modifiers = modifiers;
        Money = money;
        Manpower = manpower;
        Generals = generals;
        Admirals = admirals;
        Units = units;
        RegimentTemplates = templates;
        DiplomacyAgreements = diplomacyAgreements;
        CapitalId = capitalId;
        CurrentlyResearching = currentlyResearching;
        ResearchedTechnologies = researchedTechnologies;
        NationalIdeas = nationalIdeas;
        ConsumableGoods = consumableGoods;
    }


    [JsonConstructor]
    public CountryData()
    {
    }

    public void ResearchDayTick()
    {
        foreach (var research in CurrentlyResearching)
        {
            CurrentlyResearching[research.Key] += 1;
            if (CurrentlyResearching[research.Key] >= EngineState.MapInfo.Scenario
                    .TechnologyTrees[research.Key.X].TechnologyLevels[research.Key.Y].Technologies[research.Key.Z]
                    .ResearchTime)
                ApplyResearchedTechnology(research.Key);
        }
    }

    public void UpdateConsumableGoods()
    {
        foreach (var good in ConsumableGoods)
        {
            var consumableGood = (EngineState.MapInfo.Scenario.Goods[good.Key] as ConsumableGood);
            if (Good.CheckIfMeetsRequirements(Capital!.Resources, Good.DefaultGoods(
                    EngineState.MapInfo.Scenario.Goods.Length,
                    new Dictionary<int, double>()
                    {
                        { good.Key, consumableGood!.ConsumptionPerMonthToActivateBonus }
                    })))
            {
                Capital!.Resources =
                    Good.DecreaseGoodsByGoods(Capital!.Resources, Good.DefaultGoods(
                        EngineState.MapInfo.Scenario.Goods.Length,
                        new Dictionary<int, double>()
                        {
                            { good.Key, consumableGood!.ConsumptionPerMonthToActivateBonus }
                        }));
                ConsumableGoods[good.Key] = true;
            }
            else
            {
                ConsumableGoods[good.Key] = false;
            }
        }
    }

    public void MonthMoneyDecrease()
    {
        Money -= EngineState.MapInfo.Scenario.Settings.MoneyConsumptionPerMonthColony(EngineState.MapInfo.
            MapProvinces(ProvinceTypes.UncolonizedProvinces).Count(d =>
                (d as UncolonizedProvinceData).CurrentlyColonizedByCountry != -1 &&
                (d as UncolonizedProvinceData).CurrentlyColonizedByCountry == Id));

        Money -=
            EngineState.MapInfo.Scenario.Settings.MoneyConsumptionPerResearch(CurrentlyResearching.Count);
    }

    public void ApplyResearchedTechnology(Vector3I technologyId)
    {
        CurrentlyResearching.Remove(technologyId);
        ResearchedTechnologies[technologyId.X][technologyId.Y][technologyId.Z] = true;
    }

    public Modifiers ConsumableGoodsModifiers
    {
        get
        {
            var modifiers = Modifiers.DefaultModifiers();
            foreach (var goodId in ConsumableGoods.Where(d => d.Value))
            {
                var good = EngineState.MapInfo.Scenario.Goods[goodId.Key] as ConsumableGood;
                modifiers += good.Modifiers;
            }

            return modifiers;
        }
    }

    public UncolonizedProvinceData[] GetAvailibaleProvincesToColonize()
    {
        var a = new List<UncolonizedProvinceData>();
        foreach (UncolonizedProvinceData uncolonizedProvinceData in EngineState.MapInfo.MapProvinces(ProvinceTypes
                     .UncolonizedProvinces))
        {
            if (uncolonizedProvinceData.CanBeColonizedByCountry(Id))
                a.Add(uncolonizedProvinceData);
        }

        return a.ToArray();
    }
}