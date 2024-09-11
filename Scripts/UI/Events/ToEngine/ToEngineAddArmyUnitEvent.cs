using EuropeDominationDemo.Scripts.Scenarios.Army;

namespace EuropeDominationDemo.Scripts.UI.Events.ToEngine;

public struct ToEngineAddArmyUnitEvent : ToEngine
{
    public ArmyUnitData ArmyUnitData;

    public ToEngineAddArmyUnitEvent(ArmyUnitData armyUnitData)
    {
        ArmyUnitData = armyUnitData;
    }
}