using System;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using Newtonsoft.Json;

namespace EuropeDominationDemo.Scripts.Scenarios;

[Serializable]
public class WaterTransportationRoute : TransportationRoute
{
    public WaterTransportationRoute(int provinceIdTo, int provinceIdFrom, int transportationGood, double amount) :
        base(provinceIdTo, provinceIdFrom, transportationGood, amount)
    {
    }

    [JsonConstructor]
    public WaterTransportationRoute()
    {
        
    }
}