using System;
using System.Text.Json.Serialization;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

[Serializable]
public class HeavyShipTemplate : ShipTemplate
{
    public HeavyShipTemplate(string name, int owner) : base(name, owner)
    {
    }
    [JsonConstructor]
    public HeavyShipTemplate()
    {
    }
}