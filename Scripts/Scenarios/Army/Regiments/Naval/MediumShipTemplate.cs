using System;
using System.Text.Json.Serialization;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

[Serializable]
public class MediumShipTemplate : ShipTemplate
{
    public MediumShipTemplate(string name, int id,  int owner) : base(name, id, owner)
    {
    }
    [JsonConstructor]
    public MediumShipTemplate()
    {
    }
}