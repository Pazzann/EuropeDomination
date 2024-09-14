namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class Dockyard : SpecialBuilding
{
    public int RouteId = 0;
    public override int Cost => 100;
    public override int TimeToBuild => 100;
    public WaterTransportationRoute[] WaterTransportationRoutes;

    public Dockyard(int buildingTime, bool isFinished, WaterTransportationRoute[] waterTransportationRoutes) :
        base( buildingTime, isFinished)
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