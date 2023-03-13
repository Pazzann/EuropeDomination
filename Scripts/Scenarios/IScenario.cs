using System.Collections.Generic;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public abstract class IScenario
{
    public Dictionary<string, int> Countries;
    public Vector3[] CountriesColors;
    public string[] CountriesNames;
    public int ProvinceCount { get; set; }
    public ProvinceData[] Map { get; set; }
}