using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Units;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class MapData
{
    public Scenario Scenario;
    public MapTypes CurrentMapMode = MapTypes.Political;
    public List<ArmyUnit> CurrentSelectedUnits = new List<ArmyUnit> { };
    public int CurrentSelectedProvinceId = -2;

    private Vector3[] _mapColors
    {
        get
        {
            Vector3[] colors = new Vector3[Scenario.Map.Length];
            for (int i = 0; i < Scenario.Map.Length; i++)
            {
                if (Scenario.Map[i] is LandProvinceData landData)
                    colors[i] = Scenario.Countries[landData.Owner].Color;
                if (Scenario.Map[i] is SeaProvinceData)
                    colors[i] = Scenario.WaterColor;
                if (Scenario.Map[i] is WastelandProvinceData wastelandData)
                    colors[i] = Scenario.WastelandProvinceColors[wastelandData.Id];
                if (Scenario.Map[i] is UncolonizedProvinceData)
                    colors[i] = Scenario.UncolonizedColor;
            }

            return colors;
        }
    }

    public Vector3[] MapColors
    {
        get
        {
            switch (CurrentMapMode)
            {
                case MapTypes.Political:
                    return _mapColors;
                case MapTypes.Terrain:
                {
                    Vector3[] colors = new Vector3[Scenario.Map.Length];
                    for (int i = 0; i < Scenario.Map.Length; i++)
                    {
                        if (Scenario.Map[i] is LandProvinceData landData)
                            colors[i] = TerrainColors.Colors[(int)landData.Terrain];
                        else
                            colors[i] = new Vector3(0.1f, 0.1f, 0.1f);
                    }

                    return colors;
                }
                case MapTypes.Goods:
                {
                    Vector3[] colors = new Vector3[Scenario.Map.Length];
                    for (int i = 0; i < Scenario.Map.Length; i++)
                    {
                        if (Scenario.Map[i] is LandProvinceData landData)
                            colors[i] = GoodsColors.Colors[(int)landData.Good];
                        else
                            colors[i] = new Vector3(0.1f, 0.1f, 0.1f);
                    }

                    return colors;
                }
                case MapTypes.Trade:
                    return _mapColors;
                case MapTypes.Development:
                    return _mapColors;
                case MapTypes.Factories:
                    return _mapColors;
                case MapTypes.TransportationSelection:
                    Vector3[] selectionColors = new Vector3[Scenario.Map.Length];
                    for (int i = 0; i < Scenario.Map.Length; i++)
                    {
                        if (Scenario.Map[i] is LandProvinceData landData &&
                            landData.Owner == EngineState.PlayerCountryId)
                        {
                            if (landData.Id == CurrentSelectedProvinceId)
                                selectionColors[i] = MapDefaultColors.OwnProvince;
                            else
                                selectionColors[i] = MapDefaultColors.Selectable;
                        }
                        else
                            selectionColors[i] = MapDefaultColors.Unselectable;
                    }
                    return selectionColors;
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