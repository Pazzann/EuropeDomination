using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

public class ArmyInfantryRegimentTemplate : ArmyRegimentTemplate
{
    public InfantryWeapon Weapon { get; set; }
    public Helmet Helmet { get; set; }
    public Armor Armor { get; set; }
    public Boots Boots { get; set; }
    public AdditionalSlotGood Additional { get; set; }

    public ArmyInfantryRegimentTemplate(string name, int id, int owner, InfantryWeapon weapon, Helmet helmet, Armor armor,
        Boots boots, AdditionalSlotGood additionalSlotGood) : base(name, id, owner)
    {
        Weapon = weapon;
        Helmet = helmet;
        Armor = armor;
        Boots = boots;
        Additional = additionalSlotGood;
    }
    [JsonConstructor]
    public ArmyInfantryRegimentTemplate()
    {
    }

}