using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

public class ArmyArtilleryRegimentTemplate : ArmyRegimentTemplate
{
    public AdditionalSlotGood AdditionalSlot;
    public Armor Armor;
    public Boots Boots;
    public ArtilleryWeapon Weapon;
    public Wheel Wheel;

    public ArmyArtilleryRegimentTemplate(string name, int id, ArtilleryWeapon weapon, Boots boots, Armor armor,
        Wheel wheel, AdditionalSlotGood additionalSlotGood) : base(name, id)
    {
        Weapon = weapon;
        Boots = boots;
        Armor = armor;
        Wheel = wheel;
        AdditionalSlot = additionalSlotGood;
    }

    public override int TrainingTime =>
        (int)(
            (10 + (Weapon?.AdditionalTrainingTime ?? 0) + (Wheel?.AdditionalTrainingTime ?? 0) +
             (Armor?.AdditionalTrainingTime ?? 0) + (Boots?.AdditionalTrainingTime ?? 0) +
             (AdditionalSlot?.AdditionalTrainingTime ?? 0)) *
            (Weapon?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Wheel?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Boots?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Armor?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (AdditionalSlot?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f)
        );

    public override float Cost => 7f;
}