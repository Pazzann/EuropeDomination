namespace EuropeDominationDemo.Scripts.Scenarios.Buildings;


public  class Building
{
    public  string Name { get; }
    public int Id { get; }
    public int Cost { get; }
    public int TimeToBuild { get; }
    public int BuildingTime = 0;
    public bool IsFinished = false;
    


    public  Modifiers Modifiers { get; }

    public Building(string name, int id, int cost, int timeToBuild, int buildingTime, bool isFinished, Modifiers modifiers)
    {
        Name = name;
        Id = id;
        Cost = cost;
        TimeToBuild = timeToBuild;
        BuildingTime = buildingTime;
        IsFinished = isFinished;
        Modifiers = modifiers;
    }

    public Building Clone()
    {
        return new Building(Name, Id, Cost, TimeToBuild, 0, false, Modifiers);
    }


}