namespace EuropeDominationDemo.Scripts.Scenarios.Buildings;

public abstract class Building
{
    public abstract string Name { get; }
    public abstract int TimeToBuild { get; }
    public int BuildingTime = 0;
    public bool IsFinished = false;
    
    
    public abstract BuildingBonuses Multipliers { get; }
}