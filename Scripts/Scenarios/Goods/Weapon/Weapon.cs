using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;

public class Weapon : Good
{
    public Weapon(int id, string name, Vector3 color, float cost, float battleConsumption, float walkingConsumption,
        float steadyConsumption, float neededToBuildUnit, Modifiers modifiers, int additionalTrainingTime) : base(id,
        name, color, cost)
    {
        BattleConsumption = battleConsumption;
        WalkingConsumption = walkingConsumption;
        SteadyConsumption = steadyConsumption;
        NeededToBuildUnit = neededToBuildUnit;
        Modifiers = modifiers;
        AdditionalTrainingTime = additionalTrainingTime;
    }

    public float BattleConsumption { get; }
    public float WalkingConsumption { get; }
    public float SteadyConsumption { get; }
    public float NeededToBuildUnit { get; }
    public Modifiers Modifiers { get; }

    public int AdditionalTrainingTime { get; }
}