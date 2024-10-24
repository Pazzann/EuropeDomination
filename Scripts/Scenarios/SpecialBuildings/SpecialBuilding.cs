using System;
using System.Text.Json.Serialization;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

[Serializable]
[JsonDerivedType(typeof(Factory), typeDiscriminator: "factory")]
[JsonDerivedType(typeof(Dockyard), typeDiscriminator: "dockyard")]
[JsonDerivedType(typeof(MilitaryTrainingCamp), typeDiscriminator: "militaryTrainingCamp")]
[JsonDerivedType(typeof(StockAndTrade), typeDiscriminator: "stockAndTrade")]
public abstract class SpecialBuilding
{
    public abstract int TimeToBuild { get; }
    public int BuildingTime { get; set; }
    public abstract int Cost { get; }
    public bool IsFinished { get; set; }
    
    public void DayTick()
    {
        BuildingTime++;
        if (BuildingTime >= TimeToBuild) IsFinished = true;
    }

    public SpecialBuilding( int buildingTime, bool isFinished)
    {
        BuildingTime = buildingTime;
        IsFinished = isFinished;
    }
}