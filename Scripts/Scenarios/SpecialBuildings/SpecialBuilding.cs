namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class SpecialBuilding
{
    public readonly int TimeToBuild;
    public int BuildingTime;
    public int Cost;
    public bool IsFinished;

    public SpecialBuilding(int timeToBuild, int buildingTime, bool isFinished, int cost)
    {
        TimeToBuild = timeToBuild;
        BuildingTime = buildingTime;
        IsFinished = isFinished;
        Cost = cost;
    }
}