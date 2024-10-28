using System;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

[Serializable]
public class ArmyCavalryRegimentTemplate : ArmyRegimentTemplate
{
    public InfantryWeapon Weapon{ get; set; }
    public Horse Horse{ get; set; }
    public Helmet Helmet{ get; set; }
    public Armor Armor{ get; set; }
    public AdditionalSlotGood Additional{ get; set; }

    public ArmyCavalryRegimentTemplate(string name, int owner, InfantryWeapon weapon, Horse horse, Helmet helmet,
        Armor armor, AdditionalSlotGood additionalSlotGood) : base(name, owner)
    {
        Weapon = weapon;
        Horse = horse;
        Helmet = helmet;
        Armor = armor;
        Additional = additionalSlotGood;
    }
    [JsonConstructor]
    public ArmyCavalryRegimentTemplate()
    {
    }
}