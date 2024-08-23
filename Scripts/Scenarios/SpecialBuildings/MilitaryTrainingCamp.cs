using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class MilitaryTrainingCamp : SpecialBuilding
{
    public MilitaryTrainingCamp(int timeToBuild, int buildingTime, bool isFinished) : base(SpecialBuildingTypes.MilitaryTradingCamp, timeToBuild, buildingTime, isFinished)
    {
        
    }
}