using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class Dockyard : SpecialBuilding
{
    public Dockyard(int timeToBuild, int buildingTime, bool isFinished) : base(SpecialBuildingTypes.DockYard, timeToBuild, buildingTime, isFinished)
    {
        
    }
}