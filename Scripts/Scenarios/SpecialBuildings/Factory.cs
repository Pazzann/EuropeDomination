using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings.FactoryRecipies;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class Factory : SpecialBuilding
{
    public Recipe Recipe = null;
    public int Cost = 100;
    public float ProductionRate;
    public float ProductionGrowthRate = 0.02f;
    public Factory(Recipe recipe, int buildingTime, bool isFinished, float productionRate) : base(SpecialBuildingTypes.Factory, 100, buildingTime, isFinished)
    {
        Recipe = recipe;
        ProductionRate = productionRate;
    }
}