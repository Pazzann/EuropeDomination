using System;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings.FactoryRecipies;
using EuropeDominationDemo.Scripts.Scenarios.Technology;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public abstract class Scenario : IScenario
{
    public abstract Dictionary<int, Vector3> WastelandProvinceColors { get; set; }
    public abstract Vector3 WaterColor { get; set; }
    public abstract Vector3 UncolonizedColor { get; set; }

    public abstract List<Recipe> Recipes { get; set; }
    public abstract List<Good> Goods { get; }
    public abstract List<Terrain> Terrains { get; }
    public abstract List<Building> Buildings { get; }

    public abstract List<BattleData> Battles { get; set; }

    public abstract TechnologyTree[] TechnologyTrees { get; }
    public abstract Dictionary<int, CountryData> Countries { get; }

    public abstract ProvinceData.ProvinceData[] Map { get; set; }
    public abstract DateTime Date { get; set; }

    public abstract Image MapTexture { get; set; }


    public TimeSpan Ts => new(1, 0, 0, 0);

    public ProvinceData.ProvinceData[] CountryProvinces(int countryId)
    {
        return Map.Where(t => t is LandColonizedProvinceData data && countryId == data.Owner).ToArray();
    }

    public List<List<List<bool>>> GenerateTechnologyArray()
    {
        var technologyArray = new List<List<List<bool>>>();
        
        for (int i = 0; i < TechnologyTrees.Length; i++)
        {
            technologyArray.Add(new List<List<bool>>());
            for (int j = 0; j < TechnologyTrees[i].TechnologyLevels.Count; j++)
            {
                technologyArray[i].Add(new List<bool>());
                for (int k = 0; k < TechnologyTrees[i].TechnologyLevels[j].Technologies.Count; k++)
                {
                    technologyArray[i][j].Add(false);
                }
            }
        }

        return technologyArray;
    }
}