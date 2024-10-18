using System;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;


[Serializable]
public class Dockyard : SpecialBuilding
{
    public int RouteId { get; set; }
    public override int Cost => 100;
    public override int TimeToBuild => 100;
    public WaterTransportationRoute[] WaterTransportationRoutes { get; set; }

    public Dockyard(int buildingTime, bool isFinished, WaterTransportationRoute[] waterTransportationRoutes) :
        base( buildingTime, isFinished)
    {
        WaterTransportationRoutes = waterTransportationRoutes;
        RouteId = 0;
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