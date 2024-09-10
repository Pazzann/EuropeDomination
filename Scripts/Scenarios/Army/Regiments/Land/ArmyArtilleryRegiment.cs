using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

public class ArmyArtilleryRegiment : ArmyRegiment
{
    public ArtilleryWeapon Weapon;
    public Boots Boots;
    public Armor Armor;
    public Wheel Wheel;
    public AdditionalSlotGood AdditionalSlot;

    public ArmyArtilleryRegiment(string name, int owner, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, float morale, double[] resources, BehavioralPatterns behavioralPattern,
        Modifiers modifiers) : base(name, owner, templateId, timeFromStartOfTheTraining, trainingTime, isFinished,
        manpower, morale, resources, behavioralPattern, modifiers)
    {
    }

    public override float MaxMorale => (1f + (Weapon?.Modifiers.MaxMoraleBonus ?? 0) +
                                       (Wheel?.Modifiers.MaxMoraleBonus ?? 0) +
                                       (Armor?.Modifiers.MaxMoraleBonus ?? 0) + (Boots?.Modifiers.MaxMoraleBonus ?? 0) +
                                       (AdditionalSlot?.Modifiers.MaxMoraleBonus ?? 0)) *
                                       (Weapon?.Modifiers.MaxMoraleEfficiency ?? 1.0f) *
                                       (Wheel?.Modifiers.MaxMoraleEfficiency ?? 1.0f) *
                                       (Boots?.Modifiers.MaxMoraleEfficiency ?? 1.0f) *
                                       (Armor?.Modifiers.MaxMoraleEfficiency ?? 1.0f) *
                                       (AdditionalSlot?.Modifiers.MaxMoraleEfficiency ?? 1.0f);


    public override float MoraleIncrease => 0.04f *
                                            (Weapon?.Modifiers.MoraleIncreaseEfficiency ?? 1.0f) *
                                            (Wheel?.Modifiers.MoraleIncreaseEfficiency ?? 1.0f) *
                                            (Boots?.Modifiers.MoraleIncreaseEfficiency ?? 1.0f) *
                                            (Armor?.Modifiers.MoraleIncreaseEfficiency ?? 1.0f) *
                                            (AdditionalSlot?.Modifiers.MoraleIncreaseEfficiency ?? 1.0f);

    public override float CombatReadiness { get; }
    public override int ManpowerGrowth => (int)(10 * (Weapon?.Modifiers.ManpowerIncreaseEfficiency ?? 1.0f) *
                                                (Wheel?.Modifiers.ManpowerIncreaseEfficiency ?? 1.0f) *
                                                (Boots?.Modifiers.ManpowerIncreaseEfficiency ?? 1.0f) *
                                                (Armor?.Modifiers.ManpowerIncreaseEfficiency ?? 1.0f) *
                                                (AdditionalSlot?.Modifiers.ManpowerIncreaseEfficiency ?? 1.0f));
    public override int MaxManpower => (int)((100 + (Weapon?.Modifiers.MaxManpowerBonus ?? 0) +
                                              (Wheel?.Modifiers.MaxManpowerBonus ?? 0) +
                                              (Armor?.Modifiers.MaxManpowerBonus ?? 0) + (Boots?.Modifiers.MaxManpowerBonus ?? 0) +
                                              (AdditionalSlot?.Modifiers.MaxManpowerBonus ?? 0)) *
                                             (Weapon?.Modifiers.MaxManpowerEfficiency ?? 1.0f) *
                                             (Wheel?.Modifiers.MaxManpowerEfficiency ?? 1.0f) *
                                             (Boots?.Modifiers.MaxManpowerEfficiency ?? 1.0f) *
                                             (Armor?.Modifiers.MaxManpowerEfficiency ?? 1.0f) *
                                             (AdditionalSlot?.Modifiers.MaxManpowerEfficiency ?? 1.0f));

    public override void Recalculate()
    {
    }

    public override void ChangeTemplate()
    {
    }

    public override void Consume()
    {
    }
    public override float CombatAbility{ get; }
    public override float Defense{ get; }
    public override float MovementSpeed{ get; }
    public override float SurvivalIndex{ get; }
}