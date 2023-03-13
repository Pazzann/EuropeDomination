using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class MapData
{
    public DemoScenario Scenario;
    public MapTypes CurrentMapMode = MapTypes.Political;

    private Vector3[] _mapColors
    {
        get
        {
            Vector3[] colors =  new Vector3[Scenario.ProvinceCount];
           for (int i = 0; i < Scenario.ProvinceCount; i++)
           {
               colors[i] = Scenario.CountriesColors[Scenario.Map[i].Owner];
           }
           return colors;
        }
    }

    public Vector3[] MapColors
    {
        get{
            switch (CurrentMapMode)
            {
                case MapTypes.Political:
                    return _mapColors;
                case MapTypes.Terrain:
                    return _mapColors;
                case MapTypes.Resources:
                    return _mapColors;
                default:
                    return _mapColors;
            }
        }
    }

    public MapData(DemoScenario scenario)
    {

        Scenario = scenario;
    }

}