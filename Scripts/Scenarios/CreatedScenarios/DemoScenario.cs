using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;

public class DemoScenario : IScenario
{

    public enum Countries
    {
        Green = 0,
        Blue = 1,
        Red = 2,
    }

    public Vector3[] CountriesColors = new Vector3[3]
    {
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
    };

    public string[] CountriesNames = new string[3]
    {
        "Green",
        "Blue",
        "Red"
    };
    public override int ProvinceCount { get; set; }
    public override ProvinceData[] Map { get; set; }

    public DemoScenario()
    {
        ProvinceCount = 14;
        Map = new ProvinceData[14]
        {
            new ProvinceData(0, CountriesColors, (int)Countries.Blue, "Rekyavik"),
            new ProvinceData(1, CountriesColors, (int)Countries.Blue, "Rekyavik"),
            new ProvinceData(2, CountriesColors, (int)Countries.Blue, "Rekyavik"),
            new ProvinceData(3, CountriesColors, (int)Countries.Blue, "Rekyavik"),
            new ProvinceData(4, CountriesColors, (int)Countries.Red, "Rekyavik"),
            new ProvinceData(5, CountriesColors, (int)Countries.Red, "Rekyavik"),
            new ProvinceData(6, CountriesColors, (int)Countries.Red, "Rekyavik"),
            new ProvinceData(7, CountriesColors, (int)Countries.Red, "Rekyavik"),
            new ProvinceData(8, CountriesColors, (int)Countries.Red, "Rekyavik"),
            new ProvinceData(9, CountriesColors, (int)Countries.Green, "Rekyavik"),
            new ProvinceData(10, CountriesColors, (int)Countries.Green, "Rekyavik"),
            new ProvinceData(11, CountriesColors, (int)Countries.Green, "Rekyavik"),
            new ProvinceData(12, CountriesColors, (int)Countries.Green, "Rekyavik"),
            new ProvinceData(13, CountriesColors, (int)Countries.Green, "Rekyavik"),
        };
    }
}