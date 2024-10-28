using System;
using System.Text.Json.Serialization;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

[Serializable]
public class LightShipTemplate : ShipTemplate
{
    public LightShipTemplate(string name, int owner) : base(name, owner)
    {
    }

    [JsonConstructor]
    public LightShipTemplate()
    {
    }
}