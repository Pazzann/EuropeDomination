using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

public class ArmyInfantryRegimentTemplate : ArmyRegimentTemplate
{
    public InfantryWeapon Weapon;
    public Helmet Helmet;
    public Armor Armor;
    public Boots Boots;
    public AdditionalSlotGood AdditionalSlot;

    public ArmyInfantryRegimentTemplate(string name, int id, InfantryWeapon weapon, Helmet helmet, Armor armor,
        Boots boots, AdditionalSlotGood additionalSlotGood) : base(name, id)
    {
        Weapon = weapon;
        Helmet = helmet;
        Armor = armor;
        Boots = boots;
        AdditionalSlot = additionalSlotGood;
    }

    public override int TrainingTime
    {
        get => (int)(
            (10 + (Weapon?.AdditionalTrainingTime ?? 0) + (Helmet?.AdditionalTrainingTime ?? 0) +
             (Armor?.AdditionalTrainingTime ?? 0) + (Boots?.AdditionalTrainingTime ?? 0) +
             (AdditionalSlot?.AdditionalTrainingTime ?? 0)) *
            (Weapon?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Helmet?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Boots?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (Armor?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f) *
            (AdditionalSlot?.Modifiers.AdditionalTrainingEfficiency ?? 1.0f)
        );
    }

    public override float Cost
    {
        get => 7f;
    }
}