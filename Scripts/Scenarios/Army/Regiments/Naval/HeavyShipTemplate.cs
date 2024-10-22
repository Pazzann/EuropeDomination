using System;
using System.Text.Json.Serialization;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

[Serializable]
public class HeavyShipTemplate : ShipTemplate
{
    public HeavyShipTemplate(string name, int id, int owner) : base(name, id, owner)
    {
    }
    [JsonConstructor]
    public HeavyShipTemplate()
    {
    }
}