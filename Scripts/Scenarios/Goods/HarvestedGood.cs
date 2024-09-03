using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods;

public class HarvestedGood : Good
{
    public Modifiers Modifiers;

    public HarvestedGood(int id, string name, Vector3 color, Modifiers modifiers) : base(id, name, color)
    {
        Modifiers = modifiers;
    }
}