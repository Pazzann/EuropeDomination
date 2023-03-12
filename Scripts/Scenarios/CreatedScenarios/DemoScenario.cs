using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;

public class DemoScenario : IScenario
{
    

    public int ProvinceCount { get; set; }
    public ProvinceData[] Map { get; set; }

    public DemoScenario()
    {
        ProvinceCount = 14;
        Map = new ProvinceData[14]
        {
            new ProvinceData(0, new Vector3(1.0f, 0.0f, 0.0f), "Rekyavik"),
            new ProvinceData(1, new Vector3(1.0f, 0.0f, 0.0f), "Rekyavik"),
            new ProvinceData(2, new Vector3(1.0f, 0.0f, 0.0f), "Rekyavik"),
            new ProvinceData(3, new Vector3(1.0f, 0.0f, 0.0f), "Rekyavik"),
            new ProvinceData(4, new Vector3(0.0f, 1.0f, 0.0f), "Rekyavik"),
            new ProvinceData(5, new Vector3(0.0f, 1.0f, 0.0f), "Rekyavik"),
            new ProvinceData(6, new Vector3(0.0f, 1.0f, 0.0f), "Rekyavik"),
            new ProvinceData(7, new Vector3(0.0f, 1.0f, 0.0f), "Rekyavik"),
            new ProvinceData(8, new Vector3(0.0f, 1.0f, 0.0f), "Rekyavik"),
            new ProvinceData(9, new Vector3(0.0f, 1.0f, 0.0f), "Rekyavik"),
            new ProvinceData(10, new Vector3(0.0f, 0.0f, 1.0f), "Rekyavik"),
            new ProvinceData(11, new Vector3(0.0f, 0.0f, 1.0f), "Rekyavik"),
            new ProvinceData(12, new Vector3(0.0f, 0.0f, 1.0f), "Rekyavik"),
            new ProvinceData(13, new Vector3(0.0f, 0.0f, 1.0f), "Rekyavik"),
        };
    }
}