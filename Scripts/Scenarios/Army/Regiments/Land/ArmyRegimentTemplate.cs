using System;
using System.Text.Json.Serialization;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

[Serializable]
public abstract class ArmyRegimentTemplate : Template
{
    public ArmyRegimentTemplate(string name, int id, int owner) : base(name, id, owner)
    {
    }
    [JsonConstructor]
    public ArmyRegimentTemplate()
    {
    }
}