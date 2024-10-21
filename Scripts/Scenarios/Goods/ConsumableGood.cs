using System;
using System.Text.Json.Serialization;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods;

[Serializable]
public class ConsumableGood : Good
{
    public Modifiers Modifiers { get; init;  }
    public double ConsumptionPerMonthToActivateBonus { get; init;  }

    public ConsumableGood(int id, string name, Vector3 color, float cost, Modifiers modifiers, double consumptionPerMonthToActivateBonus) : base(id, name, color, cost)
    {
        Modifiers = modifiers;
        ConsumptionPerMonthToActivateBonus = consumptionPerMonthToActivateBonus;
    }
    [JsonConstructor]
    public ConsumableGood()
    {
        
    }
}