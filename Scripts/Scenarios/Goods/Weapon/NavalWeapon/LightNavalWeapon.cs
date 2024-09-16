using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.NavalWeapon;

public class LightNavalWeapon : NavalWeapon
{
    public LightNavalWeapon(int id, string name, Vector3 color, float cost, float battleConsumption, float walkingConsumption,
        float steadyConsumption, float neededToBuildUnit, Modifiers modifiers, int additionalTrainingTime) : base(id,
        name, color,  cost, battleConsumption, walkingConsumption, steadyConsumption, neededToBuildUnit, modifiers,
        additionalTrainingTime)
    {
    }
}