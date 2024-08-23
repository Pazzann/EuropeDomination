using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Scenarios.Army;

public class ArmyUnitData
{
    public string Name { get; set; }
    public int Owner { get; set; }
    public int CurrentProvince { get; set; }
    
    public Modifiers Modifiers { get; set; }

    public List<ArmyRegiment> Regiments { get; set; }

    public General General { get; set; }

    

    public ArmyUnitData(string name, int owner, int currentProvince, Modifiers modifiers, List<ArmyRegiment> armyRegiments, General general)
    {
        Name = name;
        Owner = owner;
        CurrentProvince = currentProvince;
        Modifiers = modifiers;
        Regiments = armyRegiments;
        General = general;
    }
}