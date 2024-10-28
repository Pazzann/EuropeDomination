using System;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

[Serializable]
public class ArmyArtilleryRegimentTemplate : ArmyRegimentTemplate
{
    public ArtilleryWeapon Weapon{ get; set; }
    public Boots Boots{ get; set; }
    public Armor Armor{ get; set; }
    public Wheel Wheel{ get; set; }
    public AdditionalSlotGood Additional{ get; set; }

    public ArmyArtilleryRegimentTemplate(string name, int owner, ArtilleryWeapon weapon, Boots boots, Armor armor,
        Wheel wheel, AdditionalSlotGood additionalSlotGood) : base(name, owner)
    {
        Weapon = weapon;
        Boots = boots;
        Armor = armor;
        Wheel = wheel;
        Additional = additionalSlotGood;
    }
    [JsonConstructor]
    public ArmyArtilleryRegimentTemplate()
    {
    }
}