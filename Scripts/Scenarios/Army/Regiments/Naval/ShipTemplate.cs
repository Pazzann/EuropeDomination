using System;
using System.Text.Json.Serialization;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

[Serializable]
public abstract class ShipTemplate : Template
{
    public ShipTemplate(string name, int id, int owner) : base(name, id, owner)
    {
    }
    [JsonConstructor]
    public ShipTemplate()
    {
    }
}