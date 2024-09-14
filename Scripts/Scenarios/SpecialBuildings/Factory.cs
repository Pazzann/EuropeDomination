using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings.FactoryRecipies;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class Factory : SpecialBuilding
{
    public override int Cost => 100;
    public override int TimeToBuild => 100;
    public float MaxProductionRate => 1.0f;
    public float ProductionGrowthRate => 0.1f;
    public float ProductionRate;
    public float CurrentAcceleration => ProductionRate / MaxProductionRate;
    public Recipe Recipe;
    public TransportationRoute TransportationRoute;

    public Factory(Recipe recipe, int buildingTime, bool isFinished, float productionRate, 
        TransportationRoute transportationRoute) : base( buildingTime, isFinished)
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