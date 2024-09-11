using EuropeDominationDemo.Scripts.Scenarios.Army;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUIAddArmyUnitEvent : GUIEvent
{
    public ArmyUnitData ArmyUnitData;

    public GUIAddArmyUnitEvent(ArmyUnitData armyUnitData)
    {
        ArmyUnitData = armyUnitData;
    }
}