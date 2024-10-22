using System;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Units;
using EuropeDominationDemo.Scripts.Utils.Math;
using Godot;
using Godot.Collections;

namespace EuropeDominationDemo.Scripts.Scenarios;

[Serializable]
public class MapData
{
    public MapTypes CurrentMapMode = MapTypes.Political;
    public int CurrentSelectedProvinceId = -2;
    public List<ArmyUnit> CurrentSelectedUnits = new();
    public Scenario Scenario;

    public MapData(Scenario scenario)
    {
        Scenario = scenario;
    }


    private Vector3[] _mapColors
    {
        get
        {
            var colors = new Vector3[Scenario.Map.Length];
            for (var i = 0; i < Scenario.Map.Length; i++)
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
                    var colors = new Vector3[Scenario.Map.Length];
                    for (var i = 0; i < Scenario.Map.Length; i++)
                        if (Scenario.Map[i] is LandProvinceData landData)
                            colors[i] = Scenario.Terrains[landData.Terrain].Color;
                        else
                            colors[i] = new Vector3(0.1f, 0.1f, 0.1f);

                    return colors;
                }
                case MapTypes.Goods:
                {
                    var colors = new Vector3[Scenario.Map.Length];
                    for (var i = 0; i < Scenario.Map.Length; i++)
                        if (Scenario.Map[i] is LandProvinceData landData)
                            colors[i] = Scenario.Goods[landData.Good].Color;
                        else
                            colors[i] = Scenario.Map[i] is WastelandProvinceData ? MapDefaultColors.Wasteland : MapDefaultColors.WaterUnselectable;

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
                    var colors = new Vector3[Scenario.Map.Length];
                    var landprovinces = MapProvinces(ProvinceTypes.LandProvinces);
                    var connections = PathFinder.CheckConnectionFromAToOthers(CurrentSelectedProvinceId, landprovinces);
                    for (var i = 0; i < landprovinces.Length; i++)
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
                        {
                            colors[landprovinces[i].Id] = MapDefaultColors.Unselectable;
                        }

                    return colors;
                }

                case MapTypes.SeaTransportationSelection:
                {
                    var colors = new Vector3[Scenario.Map.Length];
                    var coastalAndSeaProvinces = MapProvinces(ProvinceTypes.CoastalProvincesAndSeaProvinces);
                    var connections =
                        PathFinder.CheckConnectionFromAToOthers(CurrentSelectedProvinceId, coastalAndSeaProvinces);
                    for (var i = 0; i < coastalAndSeaProvinces.Length; i++)
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
                        {
                            colors[coastalAndSeaProvinces[i].Id] = MapDefaultColors.WaterUnselectable;
                        }
                        else
                        {
                            colors[coastalAndSeaProvinces[i].Id] = MapDefaultColors.Unselectable;
                        }

                    return colors;
                }

                default:
                    return _mapColors;
            }
        }
    }

    public Array<bool> VisionZone
    {
        get
        {
            var visionZone = new bool[Scenario.Map.Length];
            var visible = new HashSet<int>();
            foreach (var provinceData in Scenario.Map)
                if (provinceData is LandColonizedProvinceData landData && landData.Owner == EngineState.PlayerCountryId)
                {
                    visible.Add(landData.Id);
                    foreach (var provinceDataBorderderingProvince in provinceData.BorderderingProvinces)
                        if(Scenario.Map[provinceDataBorderderingProvince] is not WastelandProvinceData) visible.Add(provinceDataBorderderingProvince);
                }

            foreach (var unit in Scenario.Countries[EngineState.PlayerCountryId].Units)
            {
                visible.Add(unit.CurrentProvince);
                foreach (var provinceDataBorderderingProvince in Scenario.Map[unit.CurrentProvince]
                             .BorderderingProvinces) visible.Add(provinceDataBorderderingProvince);
            }

            for (var i = 0; i < visionZone.Length; i++) visionZone[i] = visible.Contains(i);
            return new Array<bool>(visionZone);
        }
    }

    public ProvinceData.ProvinceData[] MapProvinces(ProvinceTypes provinceTypes, int countryId = 0)
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
                    (d is LandColonizedProvinceData && d.BorderderingProvinces
                        .Where(i => Scenario.Map[i] is SeaProvinceData)
                        .ToArray().Length > 0) || d is SeaProvinceData).ToArray();
            case ProvinceTypes.CoastalProvinces:
                return Scenario.Map.Where(d =>
                    d is LandColonizedProvinceData && d.BorderderingProvinces
                        .Where(i => Scenario.Map[i] is SeaProvinceData)
                        .ToArray().Length > 0).ToArray();
            case ProvinceTypes.CountryProvinces:
                return Scenario.Map.Where(d =>
                    d is LandColonizedProvinceData landData&& landData.Owner == countryId)
                    .ToArray();
            case ProvinceTypes.CountryProvincesAndBordering:
            {
                var visible = new HashSet<int>();
                foreach (var provinceData in Scenario.Map)
                    if (provinceData is LandColonizedProvinceData landData && landData.Owner == countryId)
                    {
                        visible.Add(landData.Id);
                        foreach (var provinceDataBorderderingProvince in provinceData.BorderderingProvinces)
                            visible.Add(provinceDataBorderderingProvince);
                    }
                return Scenario.Map.Where(d => visible.Contains(d.Id)).ToArray();
            }
            default:
                return Scenario.Map;
        }
    }
}