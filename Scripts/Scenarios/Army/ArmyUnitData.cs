using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

namespace EuropeDominationDemo.Scripts.Scenarios.Army;

public class ArmyUnitData : UnitData
{

    public List<ArmyRegiment> Regiments { get; set; }

    public General General { get; set; }

    public ArmyUnitData(string name, int owner, int currentProvince, Modifiers modifiers, List<ArmyRegiment> armyRegiments, General general) : base(name, owner, currentProvince, modifiers)
    {
        Regiments = armyRegiments;
        General = general;
    }
}