using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

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

    public static TransportationRoute[] FindAllRoutesFromProvince(List<TransportationRoute> allRoutes,
        int provinceIdFrom)
    {
        return allRoutes.Where(a => a.ProvinceIdFrom == provinceIdFrom).ToArray();
    }
}