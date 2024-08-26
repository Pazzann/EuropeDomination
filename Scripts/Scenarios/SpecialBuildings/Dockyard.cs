using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class Dockyard : SpecialBuilding
{
    public Dockyard( int buildingTime, bool isFinished) : base(SpecialBuildingTypes.DockYard, 100, buildingTime, isFinished)
    {
        
    }
}