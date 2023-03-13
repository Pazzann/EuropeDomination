using EuropeDominationDemo.Scripts.Enums;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class MapData
{
    public IScenario Scenario;
    public MapTypes CurrentMapMode = MapTypes.Political;

    private Vector3[] _mapColors
    {
        get
        {
           Vector3[] colors =  new Vector3[Scenario.ProvinceCount];
           for (int i = 0; i < Scenario.ProvinceCount; i++)
           {
               colors[i] = Scenario.Map[i].Color;
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

    public MapData(IScenario scenario)
    {
        Scenario = scenario;
    }

}