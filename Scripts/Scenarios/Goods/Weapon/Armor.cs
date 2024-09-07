using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;

public class Armor : Weapon
{
    public Armor(int id, string name, Vector3 color, float battleConsumption, float walkingConsumption, float steadyConsumption, float neededToBuildUnit, Modifiers modifiers, int additionalTrainingTime): base(id, name, color, battleConsumption, walkingConsumption, steadyConsumption, neededToBuildUnit, modifiers, additionalTrainingTime)
    {
        
    }
}