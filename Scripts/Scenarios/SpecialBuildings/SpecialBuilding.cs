using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class SpecialBuilding
{
    public readonly SpecialBuildingTypes Type;

    public SpecialBuilding(SpecialBuildingTypes type)
    {
        Type = type;
    }
}