using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public abstract class IScenario
{
    public enum Countries{};
    public Vector3[] CountriesColors;
    public string[] CountriesNames;
    public abstract int ProvinceCount { get; set; }
    public abstract ProvinceData[] Map { get; set; }
}