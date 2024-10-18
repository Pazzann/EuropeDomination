using System;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

[Serializable]
public class HeavyShipTemplate : ShipTemplate
{
    public HeavyShipTemplate(string name, int id) : base(name, id)
    {
    }

    public override int TrainingTime { get; }

    public override float Cost => 7f;
}