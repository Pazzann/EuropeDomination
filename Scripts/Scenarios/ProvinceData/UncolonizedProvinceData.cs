using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public class UncolonizedProvinceData : LandProvinceData
{
    public CountryData CurrentlyColonizedByCountry;

    public Modifiers Modifiers;
    public int SettlersCombined;
    public int SettlersNeeded;

    public UncolonizedProvinceData(int id, string name, Terrain terrain, Good good, Modifiers modifiers,
        int settlersCombined = 0, int settlersNeeded = 5000, CountryData currentlyColonizedByCountry = null) : base(id,
        name, terrain, good)
    {
        SettlersCombined = settlersCombined;
        SettlersNeeded = settlersNeeded;
        CurrentlyColonizedByCountry = currentlyColonizedByCountry;
        Modifiers = modifiers;
    }

    public int ColonyGrowth => 1000;
    
    public LandColonizedProvinceData ConvertToLandProvince()
    {
        if (CurrentlyColonizedByCountry == null)
            throw new Exception("YOU STUPID SHIT, YOU HAVEN'T EVEN COLONIZED IT");
        else
        {
            var prData =  new LandColonizedProvinceData(Id, CurrentlyColonizedByCountry.Id, Name, Terrain, Good, 1,
                Good.DefaultGoods(), new List<Building>(), Modifiers, new SpecialBuilding[3] { null, null, null },
                null);
            prData.BorderderingProvinces = BorderderingProvinces;
            prData.CenterOfWeight = CenterOfWeight;
            return prData;
            
        }
    }
}