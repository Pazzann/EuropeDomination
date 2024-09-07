using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

public class InfantryWeapon : LandWeapon
{
    public InfantryWeapon(int id, string name, Vector3 color, float battleConsumption, float walkingConsumption, float steadyConsumption, float neededToBuildUnit, Modifiers modifiers, int additionalTrainingTime): base(id, name, color, battleConsumption, walkingConsumption, steadyConsumption, neededToBuildUnit, modifiers, additionalTrainingTime)
    {
        
    }   
}