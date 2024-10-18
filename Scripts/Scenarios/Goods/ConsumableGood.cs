using System;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods;

[Serializable]
public class ConsumableGood : Good
{
    public Modifiers Modifiers { get; set; }
    public double ConsumptionPerMonthToActivateBonus { get; set; }

    public ConsumableGood(int id, string name, Vector3 color, float cost, Modifiers modifiers, double consumptionPerMonthToActivateBonus) : base(id, name, color, cost)
    {
        Modifiers = modifiers;
        ConsumptionPerMonthToActivateBonus = consumptionPerMonthToActivateBonus;
    }
}