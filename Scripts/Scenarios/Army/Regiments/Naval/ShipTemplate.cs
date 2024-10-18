using System;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

[Serializable]
public abstract class ShipTemplate : Template
{
    public ShipTemplate(string name, int id) : base(name, id)
    {
    }
}