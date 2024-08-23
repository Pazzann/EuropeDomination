using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class SpecialBuilding
{
    public readonly SpecialBuildingTypes Type;
    public readonly int TimeToBuild;
    public int BuildingTime;
    public bool IsFinished;

    public SpecialBuilding(SpecialBuildingTypes type, int timeToBuild, int buildingTime, bool isFinished)
    {
        Type = type;
        TimeToBuild = timeToBuild;
        BuildingTime = buildingTime;
        IsFinished = isFinished;
    }
}