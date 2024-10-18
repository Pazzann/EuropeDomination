using System;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

namespace EuropeDominationDemo.Scripts.Scenarios;

[Serializable]
public class WaterTransportationRoute : TransportationRoute
{
    public WaterTransportationRoute(int provinceIdTo, int provinceIdFrom, int transportationGood, double amount) :
        base(provinceIdTo, provinceIdFrom, transportationGood, amount)
    {
    }
}