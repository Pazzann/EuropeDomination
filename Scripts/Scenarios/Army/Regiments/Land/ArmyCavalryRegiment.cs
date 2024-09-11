using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

public class ArmyCavalryRegiment : ArmyRegiment
{
    public AdditionalSlotGood AdditionalSlot;
    public Armor Armor;
    public Helmet Helmet;
    public Horse Horse;
    public InfantryWeapon Weapon;

    public ArmyCavalryRegiment(string name, int owner, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, float morale, double[] resources, BehavioralPatterns behavioralPattern,
        Modifiers modifiers) : base(name, owner, templateId, timeFromStartOfTheTraining, trainingTime, isFinished,
        manpower, morale, resources, behavioralPattern, modifiers)
    {
    }

    public override float MaxMorale => (1f + (Weapon?.Modifiers.MaxMoraleBonus ?? 0) +
                                        (Helmet?.Modifiers.MaxMoraleBonus ?? 0) +
                                        (Armor?.Modifiers.MaxMoraleBonus ?? 0) +
                                        (Horse?.Modifiers.MaxMoraleBonus ?? 0) +
                                        (AdditionalSlot?.Modifiers.MaxMoraleBonus ?? 0)) *
                                       (Weapon?.Modifiers.MaxMoraleEfficiency ?? 1.0f) *
                                       (Helmet?.Modifiers.MaxMoraleEfficiency ?? 1.0f) *
                                       (Horse?.Modifiers.MaxMoraleEfficiency ?? 1.0f) *
                                       (Armor?.Modifiers.MaxMoraleEfficiency ?? 1.0f) *
                                       (AdditionalSlot?.Modifiers.MaxMoraleEfficiency ?? 1.0f);

    public override float MoraleIncrease => 0.04f *
                                            (Weapon?.Modifiers.MoraleIncreaseEfficiency ?? 1.0f) *
                                            (Helmet?.Modifiers.MoraleIncreaseEfficiency ?? 1.0f) *
                                            (Horse?.Modifiers.MoraleIncreaseEfficiency ?? 1.0f) *
                                            (Armor?.Modifiers.MoraleIncreaseEfficiency ?? 1.0f) *
                                            (AdditionalSlot?.Modifiers.MoraleIncreaseEfficiency ?? 1.0f);

    public override float CombatReadiness { get; }

    public override int ManpowerGrowth => (int)(10 * (Weapon?.Modifiers.ManpowerIncreaseEfficiency ?? 1.0f) *
                                                (Helmet?.Modifiers.ManpowerIncreaseEfficiency ?? 1.0f) *
                                                (Horse?.Modifiers.ManpowerIncreaseEfficiency ?? 1.0f) *
                                                (Armor?.Modifiers.ManpowerIncreaseEfficiency ?? 1.0f) *
                                                (AdditionalSlot?.Modifiers.ManpowerIncreaseEfficiency ?? 1.0f));

    public override int MaxManpower => (int)((100 + (Weapon?.Modifiers.MaxManpowerBonus ?? 0) +
                                              (Helmet?.Modifiers.MaxManpowerBonus ?? 0) +
                                              (Armor?.Modifiers.MaxManpowerBonus ?? 0) +
                                              (Horse?.Modifiers.MaxManpowerBonus ?? 0) +
                                              (AdditionalSlot?.Modifiers.MaxManpowerBonus ?? 0)) *
                                             (Weapon?.Modifiers.MaxManpowerEfficiency ?? 1.0f) *
                                             (Helmet?.Modifiers.MaxManpowerEfficiency ?? 1.0f) *
                                             (Horse?.Modifiers.MaxManpowerEfficiency ?? 1.0f) *
                                             (Armor?.Modifiers.MaxManpowerEfficiency ?? 1.0f) *
                                             (AdditionalSlot?.Modifiers.MaxManpowerEfficiency ?? 1.0f));

    public override float CombatAbility { get; }
    public override float Defense { get; }
    public override float MovementSpeed { get; }
    public override float SurvivalIndex { get; }

    public override void Recalculate()
    {
    }

    public override void ChangeTemplate()
    {
    }

    public override void Consume()
    {
    }
}