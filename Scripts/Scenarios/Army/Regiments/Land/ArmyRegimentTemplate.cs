using System;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

[Serializable]
public abstract class ArmyRegimentTemplate : Template
{
    public ArmyRegimentTemplate(string name, int id) : base(name, id)
    {
    }
}