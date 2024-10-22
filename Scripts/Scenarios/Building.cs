using System;
using Newtonsoft.Json;

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

    
    [JsonConstructor]
    public Building()
    {
        
    }

    public string Name { get; init; }
    public int Id { get; init; }
    public int Cost { get; init; }
    public double[] ResourceCost { get; init; }
    public int TimeToBuild { get; init; }


    public Modifiers Modifiers { get; init; }

    public Building Clone()
    {
        return new Building(Name, Id, Cost, ResourceCost, TimeToBuild, 0, false, Modifiers);
    }
}