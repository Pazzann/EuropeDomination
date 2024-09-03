using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class TransportationRoute
{
    public int ProvinceIdFrom;
    public int ProvinceIdTo;
    public Good TransportationGood;
    public double Amount;

    public TransportationRoute(int provinceIdTo, int provinceIdFrom, Good transportationGood, double amount)
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