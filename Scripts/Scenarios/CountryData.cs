using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;
using EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class CountryData
{
    public List<Admiral> Admirals;
    public int CapitalId;
    public Vector3 Color;

    public Dictionary<int, List<DiplomacyAgreement>> DiplomacyAgreements;

    public float Money = 100;
    public int Manpower = 100;
    public Modifiers Modifiers;

    

    public string Name;
    
    public List<General> Generals;
    public List<Template> RegimentTemplates;
    public List<UnitData> Units;

    public List<List<List<bool>>> ResearchedTechnologies;
    public Dictionary<Vector3I, int> CurrentlyResearching { get; }
    public List<int> UnlockedBuildings { get; }
    public List<int> UnlockedRecipies { get; }
    
    public Modifiers NationalIdeas { get; }
    
    //bool checks if the requirments for the next month are fullfilled
    public Dictionary<int, bool> ConsumableGoods { get; }
    

    public CountryData(int id, string name, Vector3 color, Modifiers modifiers, int money, int manpower,
        List<General> generals, List<Admiral> admirals, List<UnitData> units, List<Template> templates,
        Dictionary<int, List<DiplomacyAgreement>> diplomacyAgreements, int capitalId, Dictionary<Vector3I, int> currentlyResearching, List<int> unlockedBuildings, List<int> unlockedRecipies, Modifiers nationalIdeas, Dictionary<int, bool> consumableGoods)
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
        UnlockedBuildings = unlockedBuildings;
        UnlockedRecipies = unlockedRecipies;
        NationalIdeas = nationalIdeas;
        ConsumableGoods = consumableGoods;
    }

    public void ApplyResearchedTechnology(Vector3I technologyId)
    {
        CurrentlyResearching.Remove(technologyId);
        var technology = EngineState.MapInfo.Scenario.TechnologyTrees[technologyId.X].TechnologyLevels[technologyId.Y]
            .Technologies[technologyId.Z];
        Modifiers.ApplyModifiers(technology.Modifiers);
        if (technology.BuildingToUnlock > -1)
            UnlockedBuildings.Add(technology.BuildingToUnlock);
        if(technology.RecipyToUnlock > -1)
            UnlockedRecipies.Add(technology.RecipyToUnlock);
        ResearchedTechnologies[technologyId.X][technologyId.Y][technologyId.Z] = true;
    }

    public Modifiers ConsumableGoodsModifiers
    {
        get
        {
            var modifiers = Modifiers.DefaultModifiers();
            foreach (var propertyInfo in modifiers.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                foreach (var goodId in ConsumableGoods.Where(d=> d.Value))
                {
                    var good = EngineState.MapInfo.Scenario.Goods[goodId.Key] as ConsumableGood;
                    if (propertyInfo.Name.Contains("Bonus"))
                        propertyInfo.SetValue(modifiers, (float)propertyInfo.GetValue(modifiers) + (float)propertyInfo.GetValue(good.Modifiers));
                    else
                        propertyInfo.SetValue(modifiers, (float)propertyInfo.GetValue(modifiers) * (float)propertyInfo.GetValue(good.Modifiers));
                }
            }
            
            return modifiers;
        }
    }

    public int Id { get; }
}