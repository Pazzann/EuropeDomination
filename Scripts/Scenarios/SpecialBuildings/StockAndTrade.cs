using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class StockAndTrade : SpecialBuilding
{
    public override int Cost => 100;
    public override int TimeToBuild => 100;
    public int RouteId = 0;
    public TransportationRoute[] TransportationRoutes;
    public KeyValuePair<int, double>[] SellingSlots;
    //first int is a province id
    public KeyValuePair<int, KeyValuePair<int, double>>[] BuyingSlots;

    public StockAndTrade(int buildingTime, bool isFinished, TransportationRoute[] transportationRoutes) :
        base( buildingTime, isFinished)
    {
        TransportationRoutes = transportationRoutes;
    }

    public static TransportationRoute[] DefaultRoutes()
    {
        return new TransportationRoute[10]
        {
            null, null, null, null, null, null, null, null, null, null
        };
    }

    public void SetRoute(TransportationRoute transportationRoute)
    {
        TransportationRoutes[RouteId] = transportationRoute;
    }
}