using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class StockAndTrade : SpecialBuilding
{
    public int RouteId = 0;
    public TransportationRoute[] TransportationRoutes;

    public StockAndTrade(int buildingTime, bool isFinished, int cost, TransportationRoute[] transportationRoutes) :
        base(SpecialBuildingTypes.StockAndTrade, 100, buildingTime, isFinished, cost)
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