using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

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

public class Recipe
{
    public Dictionary<Good, double> Ingredients { get; set; }
    public Good Output { get; set; }
    public float OutputAmount { get; set; }

    public Recipe(Dictionary<Good, double> ingredients, Good output, float outputAmount)
    {
        Ingredients = ingredients;
        Output = output;
        OutputAmount = outputAmount;
    }
}