using System;
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

    public ArmyArtilleryRegimentTemplate(string name, int id, ArtilleryWeapon weapon, Boots boots, Armor armor,
        Wheel wheel, AdditionalSlotGood additionalSlotGood) : base(name, id)
    {
        Weapon = weapon;
        Boots = boots;
        Armor = armor;
        Wheel = wheel;
        Additional = additionalSlotGood;
    }

    public override int TrainingTime =>
        (int)(
            (10 + (Weapon?.AdditionalTrainingTime ?? 0) + (Wheel?.AdditionalTrainingTime ?? 0) +
             (Armor?.AdditionalTrainingTime ?? 0) + (Boots?.AdditionalTrainingTime ?? 0) +
             (Additional?.AdditionalTrainingTime ?? 0)) *
            (Weapon?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Wheel?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Boots?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Armor?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Additional?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f)
        );

    public override float Cost => 7f;
}