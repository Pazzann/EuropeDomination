using System;
using System.Text.Json.Serialization;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

[Serializable]
public class MediumShipTemplate : ShipTemplate
{
    public MediumShipTemplate(string name,  int owner) : base(name, owner)
    {
    }
    [JsonConstructor]
    public MediumShipTemplate()
    {
    }
}