using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class TradingRoute : TransportationRoute
{
    public TradingRoute(int provinceIdTo, int provinceIdFrom, Good transportationGood, int amount) : base(provinceIdTo, provinceIdFrom, transportationGood, amount)
    {
    }
}