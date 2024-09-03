using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods;

public class ConsumableGood : Good
{
    public Modifiers Modifiers;

    public ConsumableGood(int id, string name, Vector3 color, Modifiers modifiers) : base(id, name, color)
    {
        Modifiers = modifiers;
    }
}