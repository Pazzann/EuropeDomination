using System;
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

    public ArmyCavalryRegimentTemplate(string name, int id, InfantryWeapon weapon, Horse horse, Helmet helmet,
        Armor armor, AdditionalSlotGood additionalSlotGood) : base(name, id)
    {
        Weapon = weapon;
        Horse = horse;
        Helmet = helmet;
        Armor = armor;
        Additional = additionalSlotGood;
    }

    public override int TrainingTime =>
        (int)(
            (10 + (Weapon?.AdditionalTrainingTime ?? 0) + (Helmet?.AdditionalTrainingTime ?? 0) +
             (Armor?.AdditionalTrainingTime ?? 0) + (Horse?.AdditionalTrainingTime ?? 0) +
             (Additional?.AdditionalTrainingTime ?? 0)) *
            (Weapon?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Helmet?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Horse?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Armor?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Additional?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f)
        );

    public override float Cost => 7f;
}