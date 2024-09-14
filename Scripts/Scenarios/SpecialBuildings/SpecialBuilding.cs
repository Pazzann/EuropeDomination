namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public abstract class SpecialBuilding
{
    public abstract int TimeToBuild { get; }
    public int BuildingTime;
    public abstract int Cost { get; }
    public bool IsFinished;

    public SpecialBuilding( int buildingTime, bool isFinished)
    {
        BuildingTime = buildingTime;
        IsFinished = isFinished;
    }
}