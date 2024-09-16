using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;

public class Helmet : Weapon
{
    public Helmet(int id, string name, Vector3 color, float cost, float battleConsumption, float walkingConsumption,
        float steadyConsumption, float neededToBuildUnit, Modifiers modifiers, int additionalTrainingTime) : base(id,
        name, color, cost, battleConsumption, walkingConsumption, steadyConsumption, neededToBuildUnit, modifiers,
        additionalTrainingTime)
    {
    }
}