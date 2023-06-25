namespace EuropeDominationDemo.Scripts.Scenarios.Buildings;

public abstract class Building
{
    public abstract string Name { get; }
    public abstract int TimeToBuild { get; }
    public int BuildingTime = 0;
    public bool IsFinished = false;
    public abstract int Cost { get; }
    public abstract int ID { get; }

    public abstract Modifiers Modifiers { get; }

    public static readonly Building[] Buildings = new Building[] { new Empty(), new Workshop() };

}