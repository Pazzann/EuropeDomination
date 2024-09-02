using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class Dockyard : SpecialBuilding
{
    public int RouteId = 0;
    public WaterTransportationRoute[] WaterTransportationRoutes;
    public Dockyard( int buildingTime, bool isFinished, int cost, WaterTransportationRoute[] waterTransportationRoutes) : base(SpecialBuildingTypes.DockYard, 100, buildingTime, isFinished, cost)
    {
        WaterTransportationRoutes = waterTransportationRoutes;
    }

    public void SetRoute(TransportationRoute transportationRoute)
    {
        WaterTransportationRoutes[RouteId] = (WaterTransportationRoute)transportationRoute;
    }
    
    public static WaterTransportationRoute[] DefaultWaterTransportationRoutes()
    {
        return new WaterTransportationRoute[5] { null, null, null, null, null };
    }
}