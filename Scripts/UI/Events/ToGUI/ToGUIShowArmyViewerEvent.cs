using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Units;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public struct ToGUIShowArmyViewerEvent : ToGUIEvent
{
    public List<ArmyUnit> ArmyUnits;

    public ToGUIShowArmyViewerEvent(List<ArmyUnit> armyUnits)
    {
        ArmyUnits = armyUnits;
    }
}