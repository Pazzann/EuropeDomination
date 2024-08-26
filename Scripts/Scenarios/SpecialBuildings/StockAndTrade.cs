using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class StockAndTrade : SpecialBuilding
{
    public StockAndTrade(int buildingTime, bool isFinished) : base(SpecialBuildingTypes.StockAndTrade, 100, buildingTime, isFinished)
    {
        
    }
}