using System.Collections.Generic;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public interface IScenario
{
    public Dictionary<string, int> Countries { get; }
    public Vector3[] CountriesColors { get; }
    public string[] CountriesNames { get; }
    public int ProvinceCount { get; }
    public ProvinceData[] Map { get; set; }
}