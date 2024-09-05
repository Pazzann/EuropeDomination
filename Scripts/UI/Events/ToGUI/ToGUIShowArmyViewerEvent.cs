using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Units;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGUIShowArmyViewerEvent : ToGUIEvent
{
    public List<ArmyUnit> ArmyUnits;

    public ToGUIShowArmyViewerEvent(List<ArmyUnit> armyUnits)
    {
        ArmyUnits = armyUnits;
    }
}