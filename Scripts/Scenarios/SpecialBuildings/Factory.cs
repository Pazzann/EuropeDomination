using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings.FactoryRecipies;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class Factory : SpecialBuilding
{
    public int Cost = 100;
    public float MaxProductionRate = 1.0f;
    public float ProductionGrowthRate = 0.1f;
    public float ProductionRate;
    public Recipe Recipe;
    public TransportationRoute TransportationRoute;

    public Factory(Recipe recipe, int buildingTime, bool isFinished, float productionRate, int cost,
        TransportationRoute transportationRoute) : base(100, buildingTime, isFinished, cost)
    {
        Recipe = recipe;
        ProductionRate = productionRate;
        TransportationRoute = transportationRoute;
    }

    public void SetRoute(TransportationRoute transportationRoute)
    {
        TransportationRoute = transportationRoute;
    }
}