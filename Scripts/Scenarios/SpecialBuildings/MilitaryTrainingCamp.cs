using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class MilitaryTrainingCamp : SpecialBuilding
{
    public MilitaryTrainingCamp( int buildingTime, bool isFinished, int cost) : base(SpecialBuildingTypes.MilitaryTradingCamp, 100, buildingTime, isFinished, cost)
    {
        
    }
}