using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

[Serializable]
[JsonDerivedType(typeof(TransportationRoute), typeDiscriminator: "transportationRoute")]
[JsonDerivedType(typeof(WaterTransportationRoute), typeDiscriminator: "waterTransportationRoute")]
public class TransportationRoute
{
    public double Amount { get; set; }
    public int ProvinceIdFrom { get; set; }
    public int ProvinceIdTo { get; set; }
    public int TransportationGood { get; set; }

    public TransportationRoute(int provinceIdTo, int provinceIdFrom, int transportationGood, double amount)
    {
        ProvinceIdTo = provinceIdTo;
        ProvinceIdFrom = provinceIdFrom;
        TransportationGood = transportationGood;
        Amount = amount;
    }
    [JsonConstructor]
    public TransportationRoute()
    {
        
    }

    public static TransportationRoute[] FindAllRoutesFromProvince(List<TransportationRoute> allRoutes,
        int provinceIdFrom)
    {
        return allRoutes.Where(a => a.ProvinceIdFrom == provinceIdFrom).ToArray();
    }

    public void TransportOnce(Modifiers modifiers)
    {
        LandColonizedProvinceData provinceFrom =
            EngineState.MapInfo.Scenario.Map[ProvinceIdFrom] as LandColonizedProvinceData;
        LandColonizedProvinceData provinceTo =
            EngineState.MapInfo.Scenario.Map[ProvinceIdTo] as LandColonizedProvinceData;
        
        var diff = provinceFrom.Resources[TransportationGood] -
                   Mathf.Max(provinceFrom.Resources[TransportationGood] - Amount, 0);
        
        provinceFrom.Resources[provinceFrom.Good] -= diff;
        provinceTo.Resources[TransportationGood] += diff;
    }
}