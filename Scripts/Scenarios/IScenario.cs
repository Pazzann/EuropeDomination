namespace EuropeDominationDemo.Scripts.Scenarios;

public interface IScenario
{
    public int ProvinceCount { get; set; }
    public ProvinceData[] Map { get; set; }
}