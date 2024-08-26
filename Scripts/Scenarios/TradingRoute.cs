﻿using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class TradingRoute : TransportationRoute
{
    public TradingRoute(int provinceIdTo, int provinceIdFrom, Good transportationGood, int amount) : base(provinceIdTo, provinceIdFrom, transportationGood, amount)
    {
    }
}