using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods;

public class ConsumableGood : Good
{
    public Modifiers Modifiers;
    public double ConsumptionPerMonthToActivateBonus;

    public ConsumableGood(int id, string name, Vector3 color, Modifiers modifiers, double consumptionPerMonthToActivateBonus) : base(id, name, color)
    {
        Modifiers = modifiers;
        ConsumptionPerMonthToActivateBonus = consumptionPerMonthToActivateBonus;
    }
}