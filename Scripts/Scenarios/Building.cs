using System;

namespace EuropeDominationDemo.Scripts.Scenarios.Buildings;

[Serializable]
public class Building
{
    public int BuildingTime { get; set; }
    public bool IsFinished { get; set; }

    public Building(string name, int id, int cost, double[] resourceCost, int timeToBuild, int buildingTime,
        bool isFinished, Modifiers modifiers)
    {
        Name = name;
        Id = id;
        Cost = cost;
        ResourceCost = resourceCost;
        TimeToBuild = timeToBuild;
        BuildingTime = buildingTime;
        IsFinished = isFinished;
        Modifiers = modifiers;
    }

    public string Name { get; }
    public int Id { get; }
    public int Cost { get; }
    public double[] ResourceCost { get; }
    public int TimeToBuild { get; }


    public Modifiers Modifiers { get; }

    public Building Clone()
    {
        return new Building(Name, Id, Cost, ResourceCost, TimeToBuild, 0, false, Modifiers);
    }
}