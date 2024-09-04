using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Units;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGUIEventShowArmyViewerEvent : ToGUIEvent
{
    public List<ArmyUnit> ArmyUnits;

    public ToGUIEventShowArmyViewerEvent(List<ArmyUnit> armyUnits)
    {
        ArmyUnits = armyUnits;
    }
}