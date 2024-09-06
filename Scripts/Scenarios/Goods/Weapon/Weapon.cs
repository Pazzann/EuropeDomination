using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;

public class Weapon : Good
{
    public float BattleConsumption { get; }
    public float WalkingConsumption { get; }
    public float SteadyConsumption { get; }
    public float NeededToBuildUnit { get; }
    public Modifiers Modifiers { get; }
    
    public Weapon(int id, string name, Vector3 color): base(id, name, color)
    {
        
    }
}