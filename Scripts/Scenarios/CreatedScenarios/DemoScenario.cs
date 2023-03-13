using System.Collections.Generic;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;

public class DemoScenario : IScenario
{

    public Dictionary<string, int> Countries = new Dictionary<string, int>()
    {
        { "Green", 0 },
        { "Blue", 1 },
        { "Red", 2 }
    };

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

    public int ProvinceCount
    {
        get
        {
            return 14;
        }
    }

    public ProvinceData[] Map { get; set; }

    public DemoScenario()
    {
        Map = new ProvinceData[14]
        {
            new ProvinceData(0, Countries["Blue"], "Rekyavik"),
            new ProvinceData(1,  Countries["Blue"], "Rekyavik"),
            new ProvinceData(2,  Countries["Blue"], "Rekyavik"),
            new ProvinceData(3,  Countries["Blue"], "Rekyavik"),
            new ProvinceData(4,  Countries["Red"], "Rekyavik"),
            new ProvinceData(5,  Countries["Red"], "Rekyavik"),
            new ProvinceData(6,  Countries["Red"], "Rekyavik"),
            new ProvinceData(7,  Countries["Red"], "Rekyavik"),
            new ProvinceData(8,  Countries["Red"], "Rekyavik"),
            new ProvinceData(9,  Countries["Green"], "Rekyavik"),
            new ProvinceData(10, Countries["Green"], "Rekyavik"),
            new ProvinceData(11, Countries["Green"], "Rekyavik"),
            new ProvinceData(12,  Countries["Green"], "Rekyavik"),
            new ProvinceData(13, Countries["Green"], "Rekyavik"),
        };
    }
}