using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public interface IScenario
{
    public Dictionary<int, CountryData> Countries { get; }
    public ProvinceData.ProvinceData[] Map { get; set; }
    
    public ProvinceData.ProvinceData[] CountryProvinces(int countryId);

    public List<ArmyUnitData> ArmyUnits { get; set; }
    
    public DateTime Date { get; set; }
    public Image MapTexture { get; set; } 
    
    
    public TimeSpan Ts { get; }
}