using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class MapData
{
    public Scenario Scenario;
    public MapTypes CurrentMapMode = MapTypes.Political;

    private Vector3[] _mapColors
    {
        get
        { 
            Vector3[] colors =  new Vector3[Scenario.Map.Length];
           for (int i = 0; i < Scenario.Map.Length; i++)
           {
               colors[i] = Scenario.Countries[Scenario.Map[i].Owner].Color;
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
                {
                    Vector3[] colors =  new Vector3[Scenario.Map.Length];
                    for (int i = 0; i < Scenario.Map.Length; i++)
                    {
                        colors[i] = TerrainColors.Colors[(int)Scenario.Map[i].Terrain];
                    }
                    return colors;
                }
                case MapTypes.Goods:
                {
                    Vector3[] colors =  new Vector3[Scenario.Map.Length];
                    for (int i = 0; i < Scenario.Map.Length; i++)
                    {
                        colors[i] = GoodsColors.Colors[(int)Scenario.Map[i].Good];
                    }
                    return colors;
                }
                case MapTypes.Trade:
                    return _mapColors;
                case MapTypes.Development:
                    return _mapColors;
                case MapTypes.Factories:
                    return _mapColors;
                default:
                    return _mapColors;
            }
        }
    }

    public MapData(Scenario scenario)
    {
        Scenario = scenario;
    }

}