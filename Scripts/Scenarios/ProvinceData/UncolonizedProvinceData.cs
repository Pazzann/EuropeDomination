using System;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

[Serializable]
public class UncolonizedProvinceData : LandProvinceData
{
    public int CurrentlyColonizedByCountry { get; set; }
    public Modifiers Modifiers { get; set; }
    public int SettlersCombined { get; set; }
    public int SettlersNeeded { get; set; }

    public UncolonizedProvinceData(int id, string name, int terrain, int good, Modifiers modifiers,
        int settlersCombined = 0, int settlersNeeded = 5000, int currentlyColonizedByCountry = -1,
        int[] borderderingProvinces = null, 
        Vector2 centerOfWeight = new ()
    ) : base(id, name, terrain, good, borderderingProvinces, centerOfWeight)
    {
        SettlersCombined = settlersCombined;
        SettlersNeeded = settlersNeeded;
        CurrentlyColonizedByCountry = currentlyColonizedByCountry;
        Modifiers = modifiers;
    }
    
    
    public LandColonizedProvinceData ConvertToLandProvince()
    {
        if (CurrentlyColonizedByCountry == -1)
            throw new Exception("YOU STUPID SHIT, YOU HAVEN'T EVEN COLONIZED IT");
        var prData =  new LandColonizedProvinceData(Id, CurrentlyColonizedByCountry, Name, Terrain, Good, 1,
            Goods.Good.DefaultGoods(EngineState.MapInfo.Scenario.Goods.Length), new List<Building>(), Modifiers, new SpecialBuilding[3] { null, null, null },
            null);
        prData.BorderderingProvinces = BorderderingProvinces;
        prData.CenterOfWeight = CenterOfWeight;
        return prData;
    }

    public bool CanBeColonizedByCountry(int countryId)
    {
        var country = EngineState.MapInfo.Scenario.Countries[countryId];
        var a = CurrentlyColonizedByCountry == -1;
            
        //Some Crazy condition
        if (!EngineState.MapInfo.MapProvinces(ProvinceTypes.CountryProvincesAndBordering, countryId).Contains(this))
            if (!EngineState.MapInfo.MapProvinces(ProvinceTypes.CountryProvinces, countryId)
                    .Where(d => (d as LandColonizedProvinceData).SpecialBuildings.Any(b => b is Dockyard))
                    .Any(d =>
                    {
                        var searchPr = EngineState.MapInfo.MapProvinces(ProvinceTypes.SeaProvinces).ToList();
                        searchPr.Add(d);
                        searchPr.Add(this);
                        return PathFinder.CheckConnectionFromAToB(d.Id, Id,
                                   searchPr.ToArray()) &&
                               PathFinder.FindPathFromAToB(d.Id, Id, searchPr.ToArray()
                               ).Length < EngineState.MapInfo.Scenario.Settings.NavalColonizationRange;
                    }))
                a = false;
        if (country.Money < EngineState.MapInfo.Scenario.Settings.InitialMoneyCostColony || country.Manpower < EngineState.MapInfo.Scenario.Settings.InitialManpowerCostColony)
            a = false;
        return a;
    }
}