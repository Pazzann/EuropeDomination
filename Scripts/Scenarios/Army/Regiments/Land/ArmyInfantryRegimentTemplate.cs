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

    public ArmyInfantryRegimentTemplate(string name, int id, InfantryWeapon weapon, Helmet helmet, Armor armor,
        Boots boots, AdditionalSlotGood additionalSlotGood) : base(name, id)
    {
        Weapon = weapon;
        Helmet = helmet;
        Armor = armor;
        Boots = boots;
        Additional = additionalSlotGood;
    }

    public override int TrainingTime =>
        (int)(
            (10 + (Weapon?.AdditionalTrainingTime ?? 0) + (Helmet?.AdditionalTrainingTime ?? 0) +
             (Armor?.AdditionalTrainingTime ?? 0) + (Boots?.AdditionalTrainingTime ?? 0) +
             (Additional?.AdditionalTrainingTime ?? 0)) *
            (Weapon?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Helmet?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Boots?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Armor?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Additional?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f)
        );

    public override float Cost => 7f;
}