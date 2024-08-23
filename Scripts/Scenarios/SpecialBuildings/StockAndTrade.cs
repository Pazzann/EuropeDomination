using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class StockAndTrade : SpecialBuilding
{
    public StockAndTrade(int timeToBuild, int buildingTime, bool isFinished) : base(SpecialBuildingTypes.StockAndTrade, timeToBuild, buildingTime, isFinished)
    {
        
    }
}