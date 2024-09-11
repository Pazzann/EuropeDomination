using EuropeDominationDemo.Scripts.Scenarios.Goods;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class WaterTransportationRoute : TransportationRoute
{
    public WaterTransportationRoute(int provinceIdTo, int provinceIdFrom, Good transportationGood, double amount) :
        base(provinceIdTo, provinceIdFrom, transportationGood, amount)
    {
    }
}