using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;

public class StockAndTrade : SpecialBuilding
{
    public StockAndTrade(int buildingTime, bool isFinished, int cost) : base(SpecialBuildingTypes.StockAndTrade, 100, buildingTime, isFinished, cost)
    {
        
    }
}