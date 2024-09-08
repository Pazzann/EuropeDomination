using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Math;
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
                if (Scenario.Map[i] is LandColonizedProvinceData landData)
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

    public ProvinceData.ProvinceData[] MapProvinces(ProvinceTypes provinceTypes)
    {
        switch (provinceTypes)
        {
            case ProvinceTypes.SeaProvinces:
                return Scenario.Map.Where(d => d is SeaProvinceData).ToArray();
            case ProvinceTypes.LandProvinces:
                return Scenario.Map.Where(d => d is LandProvinceData).ToArray();
            case ProvinceTypes.ColonizedProvinces:
                return Scenario.Map.Where(d => d is LandColonizedProvinceData).ToArray();
            case ProvinceTypes.UncolonizedProvinces:
                return Scenario.Map.Where(d => d is UncolonizedProvinceData).ToArray();
            case ProvinceTypes.CoastalProvincesAndSeaProvinces:
                return Scenario.Map.Where(d =>
                    (d is LandColonizedProvinceData && d.BorderderingProvinces.Where(i => Scenario.Map[i] is SeaProvinceData)
                        .ToArray().Length > 0) || d is SeaProvinceData).ToArray();
            case ProvinceTypes.CoastalProvinces:
                return Scenario.Map.Where(d =>
                    d is LandColonizedProvinceData && d.BorderderingProvinces.Where(i => Scenario.Map[i] is SeaProvinceData)
                        .ToArray().Length > 0).ToArray();
            default:
                return Scenario.Map;
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
                            colors[i] = landData.Good.Color;
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
                {
                    Vector3[] colors = new Vector3[Scenario.Map.Length];
                    var landprovinces = MapProvinces(ProvinceTypes.LandProvinces);
                    var connections = PathFinder.CheckConnectionFromAToOthers(CurrentSelectedProvinceId, landprovinces);
                    for (int i = 0; i < landprovinces.Length; i++)
                    {
                        if (landprovinces[i] is LandColonizedProvinceData landData &&
                            landData.Owner == EngineState.PlayerCountryId &&
                            connections[landData.Id])
                        {
                            if (landData.Id == CurrentSelectedProvinceId)
                                colors[landprovinces[i].Id] = MapDefaultColors.OwnProvince;
                            else
                                colors[landprovinces[i].Id] = MapDefaultColors.Selectable;
                        }
                        else
                            colors[landprovinces[i].Id] = MapDefaultColors.Unselectable;
                    }

                    return colors;
                }

                case MapTypes.SeaTransportationSelection:
                {
                    Vector3[] colors = new Vector3[Scenario.Map.Length];
                    var coastalAndSeaProvinces = MapProvinces(ProvinceTypes.CoastalProvincesAndSeaProvinces);
                    var connections = PathFinder.CheckConnectionFromAToOthers(CurrentSelectedProvinceId, coastalAndSeaProvinces);
                    for (int i = 0; i < coastalAndSeaProvinces.Length; i++)
                    {
                        if (coastalAndSeaProvinces[i] is LandColonizedProvinceData landData &&
                            landData.Owner == EngineState.PlayerCountryId &&
                            connections[landData.Id]
                           )
                        {
                            if (landData.Id == CurrentSelectedProvinceId)
                                colors[landData.Id] = MapDefaultColors.OwnProvince;
                            else
                                colors[landData.Id] = MapDefaultColors.Selectable;
                        }
                        else if (coastalAndSeaProvinces[i] is SeaProvinceData)
                            colors[coastalAndSeaProvinces[i].Id] = MapDefaultColors.WaterUnselectable;
                        else
                            colors[coastalAndSeaProvinces[i].Id] = MapDefaultColors.Unselectable;
                    }

                    return colors; 
                }
                    
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