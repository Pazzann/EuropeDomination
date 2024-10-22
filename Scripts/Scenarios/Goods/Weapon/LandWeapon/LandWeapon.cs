using System;
using System.Text.Json.Serialization;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

[Serializable]
public class LandWeapon : Weapon
{
    public LandWeapon(int id, string name, Vector3 color, float cost,float battleConsumption, float walkingConsumption,
        float steadyConsumption, float neededToBuildUnit, Modifiers modifiers) : base(id,
        name, color, cost,battleConsumption, walkingConsumption, steadyConsumption, neededToBuildUnit, modifiers)
    {
    }
    [JsonConstructor]
    public LandWeapon()
    {
        
    }
}