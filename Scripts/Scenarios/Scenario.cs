using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.Scenarios.Technology;
using Godot;
using Steamworks;

namespace EuropeDominationDemo.Scripts.Scenarios;

[Serializable]
public abstract class Scenario
{
    public abstract ScenarioSettings Settings { get; set; }
    public abstract Dictionary<int, Vector3> WastelandProvinceColors { get; set; }
    public abstract Vector3 WaterColor { get; set; }
    public abstract Vector3 UncolonizedColor { get; set; }
    public abstract Good[] Goods { get; }
    public abstract Recipe[] Recipes { get; set; }
    public abstract Terrain[] Terrains { get; }
    public abstract Building[] Buildings { get; }
    public abstract List<BattleData> Battles { get; set; }
    public abstract TechnologyTree[] TechnologyTrees { get; }
    public abstract Dictionary<int, CountryData> Countries { get; }
    public abstract ProvinceData.ProvinceData[] Map { get; set; }
    public abstract DateTime Date { get; set; }

    public HashSet<int> AiList = new HashSet<int>();
    //int is for countryId
    public abstract Dictionary<ulong, int> PlayerList { get; set; }
    
    public void ChangeGameMode(GameModes mode) 
    {
        switch (mode)
        {
            case GameModes.RandomSpawn:
            {
                Settings.GameMode = GameModes.RandomSpawn;
                CleanMap();
                return;
            }
            case GameModes.SelectionSpawn:
            {
                Settings.GameMode = GameModes.SelectionSpawn;
                CleanMap();
                return;
            }
            case GameModes.FullMapScenario:
            {
                Settings.GameMode = GameModes.FullMapScenario;
                CleanMap();
                return;
            }
        }
    }
    

    public void Init()
    {
        switch (Settings.GameMode)
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
                    if(!PlayerList.ContainsValue(country.Key))
                        AiList.Add(country.Key);
                    country.Value.CapitalId = countOfLandProvinces[capitalsArray[country.Value.Id]].Id;
                    var a = (UncolonizedProvinceData)Map[country.Value.CapitalId];
                    a.CurrentlyColonizedByCountry = country.Value.Id;
                    var b = a.ConvertToLandProvince();
                    b.Development = 10;
                    Map[a.Id] = b;
                    country.Value.ResearchedTechnologies = GenerateTechnologyArray();
                }
                break;
            }
            case GameModes.SelectionSpawn:
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
                    if (!PlayerList.ContainsValue(country.Key))
                    {
                        AiList.Add(country.Key);
                        country.Value.CapitalId = countOfLandProvinces[capitalsArray[country.Value.Id]].Id;
                        var a = (UncolonizedProvinceData)Map[country.Value.CapitalId];
                        a.CurrentlyColonizedByCountry = country.Value.Id;
                        var b = a.ConvertToLandProvince();
                        b.Development = 10;
                        Map[a.Id] = b;
                    }
                    country.Value.ResearchedTechnologies = GenerateTechnologyArray();
                }
                break;
            }
            case GameModes.FullMapScenario:
            {
                foreach (var country in Countries)
                {
                    if(!PlayerList.ContainsValue(country.Key))
                        AiList.Add(country.Key);
                    var a = (UncolonizedProvinceData)Map[country.Value.CapitalId];
                    a.CurrentlyColonizedByCountry = country.Value.Id;
                    var b = a.ConvertToLandProvince();
                    b.Development = 10;
                    Map[a.Id] = b;
                    country.Value.ResearchedTechnologies = GenerateTechnologyArray();
                }
                break;
            }
        }

        switch (Settings.ResourceMode)
        {
            case ResourceModes.RandomSpawn:
            {
                var harvestedGoods = Goods.Where(d => d is HarvestedGood).ToArray();
                var random = new Random();
                foreach (LandProvinceData province in Map.Where(d => d is LandProvinceData))
                {
                    province.Good = harvestedGoods[random.Next(0, harvestedGoods.Length)].Id;
                }
                break;
            }
            case ResourceModes.ScenarioSpawn:
            {
                break;
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