using System;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
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
    
    
    public abstract GameModes GameMode { get; set; }

    public HashSet<int> AiList = new HashSet<int>();
    //int is for countryId
    public abstract Dictionary<int, string> PlayerList { get; set; }


    public void ChangeGameMode(GameModes mode) 
    {
        switch (mode)
        {
            case GameModes.RandomSpawn:
            {
                GameMode = GameModes.RandomSpawn;
                CleanMap();
                return;
            }
            case GameModes.SelectionSpawn:
            {
                GameMode = GameModes.SelectionSpawn;
                CleanMap();
                return;
            }
            case GameModes.FullMapScenario:
            {
                GameMode = GameModes.FullMapScenario;
                CleanMap();
                return;
            }
        }
    }

    public void Init()
    {
        switch (GameMode)
        {
            case GameModes.RandomSpawn:
            {
                var countOfLandProvinces = Map.Where(d => d is UncolonizedProvinceData).ToArray();
                var capitals = new HashSet<int>();
                while (capitals.Count != Countries.Count)
                {
                    capitals.Add(new Random().Next(0, countOfLandProvinces.Length));
                }

                var capitalsArray = capitals.ToArray();
        

                foreach (var country in Countries)
                {
                    if(!PlayerList.ContainsKey(country.Key))
                        AiList.Add(country.Key);
                    country.Value.CapitalId = countOfLandProvinces[capitalsArray[country.Value.Id]].Id;
                    var a = (UncolonizedProvinceData)Map[country.Value.CapitalId];
                    a.CurrentlyColonizedByCountry = country.Value;
                    var b = a.ConvertToLandProvince();
                    b.Development = 10;
                    Map[a.Id] = b;
                    country.Value.ResearchedTechnologies = GenerateTechnologyArray();
                }
                return;
            }
            case GameModes.SelectionSpawn:
            {
                return;
            }
            default:
            {
                return;
            }
        }
    }

    public void CleanMap()
    {
        for (int i = 0; i < Map.Length; i++)
        {
            if (Map[i] is LandColonizedProvinceData landColonizedProvinceData)
            {
                Map[i] = new UncolonizedProvinceData(landColonizedProvinceData.Id, landColonizedProvinceData.Name, landColonizedProvinceData.Terrain, landColonizedProvinceData.Good, landColonizedProvinceData.Modifiers);
            }
        }
    }
    
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