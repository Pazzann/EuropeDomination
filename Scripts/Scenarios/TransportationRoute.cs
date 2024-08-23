using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class TransportationRoute
{
    public int ProvinceIdFrom;
    public int ProvinceIdTo;
    public Good TransportationGood;
    public int Amount;

    public TransportationRoute(int provinceIdTo, int provinceIdFrom, Good transportationGood, int amount)
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