using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Units;

namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUIMergeUnitsEvent: GUIEvent
{
    public List<ArmyUnit> UnitsToMerge;

    public GUIMergeUnitsEvent(List<ArmyUnit> unitsToMerge)
    {
        UnitsToMerge = unitsToMerge;
    }
}