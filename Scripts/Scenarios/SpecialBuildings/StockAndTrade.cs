using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

[Serializable]
public class StockAndTrade : SpecialBuilding
{
    public override int Cost => 100;

    public override int TimeToBuild => 100;
    public int RouteId { get; set; }
    public TransportationRoute[] TransportationRoutes { get; set; }
    public KeyValuePair<int, double>[] SellingSlots { get; set; }
    
    //first int is a province id; set equal to province Id if buying from internal market
    public KeyValuePair<int, KeyValuePair<int, double>>[] BuyingSlots { get; set; }

    public StockAndTrade(int buildingTime, bool isFinished, TransportationRoute[] transportationRoutes, KeyValuePair<int, double>[] sellingSlots, KeyValuePair<int, KeyValuePair<int, double>>[] buyingSlots) :
        base( buildingTime, isFinished)
    {
        TransportationRoutes = transportationRoutes;
        SellingSlots = sellingSlots;
        BuyingSlots = buyingSlots;
        RouteId = 0;
    }

    public void Transport(Modifiers modifiers)
    {
        foreach (var route in TransportationRoutes)
            if (route != null)
                route.TransportOnce(modifiers);
            
    }

    public static TransportationRoute[] DefaultRoutes()
    {
        return
        [
            null, null, null, null, null, null, null, null, null, null
        ];
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