using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class StockAndTrade : SpecialBuilding
{
    public override int Cost => 100;
    public override int TimeToBuild => 100;
    public int RouteId = 0;
    public TransportationRoute[] TransportationRoutes;
    public KeyValuePair<int, double>[] SellingSlots;
    
    //first int is a province id; set equal to province Id if buying from internal market
    public KeyValuePair<int, KeyValuePair<int, double>>[] BuyingSlots;

    public StockAndTrade(int buildingTime, bool isFinished, TransportationRoute[] transportationRoutes, KeyValuePair<int, double>[] sellingSlots, KeyValuePair<int, KeyValuePair<int, double>>[] buyingSlots) :
        base( buildingTime, isFinished)
    {
        TransportationRoutes = transportationRoutes;
        SellingSlots = sellingSlots;
        BuyingSlots = buyingSlots;
    }

    public static TransportationRoute[] DefaultRoutes()
    {
        return new TransportationRoute[10]
        {
            null, null, null, null, null, null, null, null, null, null
        };
    }

    public static KeyValuePair<int, double>[] DefaultSellingSlots()
    {
        return new KeyValuePair<int, double>[5]
        {
            new (-1, 0),
            new (-1, 0),
            new (-1, 0),
            new (-1, 0),
            new (-1, 0),
        };
    }

    public static KeyValuePair<int, KeyValuePair<int, double>>[] DefaultBuyingSlots()
    {
        return new KeyValuePair<int, KeyValuePair<int, double>>[5]
        {
            new (-1, new KeyValuePair<int, double>(-1, 0)),
            new (-1, new KeyValuePair<int, double>(-1, 0)),
            new (-1, new KeyValuePair<int, double>(-1, 0)),
            new (-1, new KeyValuePair<int, double>(-1, 0)),
            new (-1, new KeyValuePair<int, double>(-1, 0))
        };
    }

    public void SetRoute(TransportationRoute transportationRoute)
    {
        TransportationRoutes[RouteId] = transportationRoute;
    }
}