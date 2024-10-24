using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

[Serializable]
public class Factory : SpecialBuilding
{
    public override int Cost => 100;
    public override int TimeToBuild => 100;
    
    public float MaxProductionRate => 1.0f;
    public float ProductionGrowthRate => 0.1f;
    public float ProductionRate { get; set; }
    
    public float CurrentAcceleration => ProductionRate / MaxProductionRate;
    public int Recipe { get; set; }
    public TransportationRoute TransportationRoute { get; set; }

    public Factory(int recipe, int buildingTime, bool isFinished, float productionRate, 
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

[Serializable]
public class Recipe
{
    public int Id { get; set; }
    public Dictionary<int, double> Ingredients { get; set; }
    public int Output { get; set; }
    public float OutputAmount { get; set; }

    public Recipe(int id,Dictionary<int, double> ingredients, int output, float outputAmount)
    {
        Id = id;
        Ingredients = ingredients;
        Output = output;
        OutputAmount = outputAmount;
    }
}