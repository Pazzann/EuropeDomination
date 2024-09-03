using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class SpecialBuilding
{
    public readonly int TimeToBuild;
    public int BuildingTime;
    public bool IsFinished;
    public int Cost;

    public SpecialBuilding(int timeToBuild, int buildingTime, bool isFinished, int cost)
    {
        TimeToBuild = timeToBuild;
        BuildingTime = buildingTime;
        IsFinished = isFinished;
        Cost = cost;
    }
}