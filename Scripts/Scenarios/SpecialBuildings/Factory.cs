using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings.FactoryRecipies;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class Factory : SpecialBuilding
{
    public Recipe Recipe = null;
    public int Cost = 100;
    public Factory(Recipe recipe) : base(SpecialBuildingTypes.Factory)
    {
        Recipe = recipe;
    }
}