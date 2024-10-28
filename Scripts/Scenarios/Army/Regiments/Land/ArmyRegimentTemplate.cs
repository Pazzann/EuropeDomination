using System;
using System.Text.Json.Serialization;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

[Serializable]
public abstract class ArmyRegimentTemplate : Template
{
    public ArmyRegimentTemplate(string name, int owner) : base(name, owner)
    {
    }
    [JsonConstructor]
    public ArmyRegimentTemplate()
    {
    }
}