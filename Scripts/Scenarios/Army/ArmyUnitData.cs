using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

namespace EuropeDominationDemo.Scripts.Scenarios.Army;

[Serializable]
public class ArmyUnitData : UnitData
{
    public ArmyUnitData(string name, int owner, int currentProvince, Modifiers modifiers,
        List<ArmyRegiment> armyRegiments, General general, List<KeyValuePair<int, int>> movementQueue,
        int movementProgress, UnitStates unitState) : base(name, owner, currentProvince, modifiers, movementQueue,
        movementProgress, unitState)
    {
        Regiments = armyRegiments;
        General = general;
    }

    public List<ArmyRegiment> Regiments { get; set; }

    public General General { get; set; }
}