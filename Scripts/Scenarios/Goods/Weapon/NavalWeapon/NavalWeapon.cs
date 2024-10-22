using System;
using System.Text.Json.Serialization;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.NavalWeapon;

[Serializable]
public class NavalWeapon : Weapon
{
    public NavalWeapon(int id, string name, Vector3 color, float cost, float battleConsumption, float walkingConsumption,
        float steadyConsumption, float neededToBuildUnit, Modifiers modifiers) : base(id,
        name, color, cost, battleConsumption, walkingConsumption, steadyConsumption, neededToBuildUnit, modifiers)
    {
    }


    [JsonConstructor]
    public NavalWeapon()
    {
        
    }
}