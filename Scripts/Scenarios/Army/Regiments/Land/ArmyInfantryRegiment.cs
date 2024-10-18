using System;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

[Serializable]
public class ArmyInfantryRegiment : ArmyRegiment
{
    public AdditionalSlotGood AdditionalSlot { get; set; }
    public Armor Armor{ get; set; }
    public Boots Boots{ get; set; }
    public Helmet Helmet{ get; set; }
    public InfantryWeapon Weapon{ get; set; }

    public ArmyInfantryRegiment(string name, int owner, int templateId, int timeFromStartOfTheTraining,
        int trainingTime,
        bool isFinished, int manpower, float morale, double[] resources, BehavioralPatterns behavioralPattern,
        Modifiers modifiers) : base(name, owner, templateId, timeFromStartOfTheTraining, trainingTime, isFinished,
        manpower,
        morale, resources, behavioralPattern, modifiers)
    {
    }

    public override float MaxMorale
    {
        get
        {
            var maxMorale = 1f;
            if (Weapon != null)
                maxMorale += Weapon.Modifiers.MaxMoraleBonus;
            if (Armor != null)
                maxMorale += Armor.Modifiers.MaxMoraleBonus;
            if (Helmet != null)
                maxMorale += Helmet.Modifiers.MaxMoraleBonus;
            if (Boots != null)
                maxMorale += Boots.Modifiers.MaxMoraleBonus;
            if (AdditionalSlot != null)
                maxMorale += AdditionalSlot.Modifiers.MaxMoraleBonus;

            maxMorale += EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.MaxMoraleBonus;
            maxMorale += EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.MaxMoraleBonus;
            maxMorale += EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.MaxMoraleBonus;

            if (Weapon != null)
                maxMorale *= Weapon.Modifiers.MaxMoraleEfficiency;
            if (Armor != null)
                maxMorale *= Armor.Modifiers.MaxMoraleEfficiency;
            if (Helmet != null)
                maxMorale *= Helmet.Modifiers.MaxMoraleEfficiency;
            if (Boots != null)
                maxMorale *= Boots.Modifiers.MaxMoraleEfficiency;
            if (AdditionalSlot != null)
                maxMorale *= AdditionalSlot.Modifiers.MaxMoraleEfficiency;

            maxMorale *= EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.MaxMoraleEfficiency;
            maxMorale *= EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.MaxMoraleEfficiency;
            maxMorale *= EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.MaxMoraleEfficiency;

            return maxMorale;
        }
    }


    public override float CurrentMaxMorale
    {
        get
        {
            var maxMorale = 1f;
            if (Weapon != null)
                maxMorale += Weapon.Modifiers.MaxMoraleBonus *
                             (float)(Resources[Weapon.Id] / Weapon.NeededToBuildUnit > 1
                                 ? 1
                                 : Resources[Weapon.Id] / Weapon.NeededToBuildUnit);
            if (Armor != null)
                maxMorale += Armor.Modifiers.MaxMoraleBonus * (float)(Resources[Armor.Id] / Armor.NeededToBuildUnit > 1
                    ? 1
                    : Resources[Armor.Id] / Armor.NeededToBuildUnit);
            if (Helmet != null)
                maxMorale += Helmet.Modifiers.MaxMoraleBonus *
                             (float)(Resources[Helmet.Id] / Helmet.NeededToBuildUnit > 1
                                 ? 1
                                 : Resources[Helmet.Id] / Helmet.NeededToBuildUnit);
            if (Boots != null)
                maxMorale += Boots.Modifiers.MaxMoraleBonus * (float)(Resources[Boots.Id] / Boots.NeededToBuildUnit > 1
                    ? 1
                    : Resources[Boots.Id] / Boots.NeededToBuildUnit);
            if (AdditionalSlot != null)
                maxMorale += AdditionalSlot.Modifiers.MaxMoraleBonus *
                             (float)(Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit > 1
                                 ? 1
                                 : Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit);

            maxMorale += EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.MaxMoraleBonus;
            maxMorale += EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.MaxMoraleBonus;
            maxMorale += EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.MaxMoraleBonus;

            if (Weapon != null)
                maxMorale *= Weapon.Modifiers.MaxMoraleEfficiency *
                             (float)(Resources[Weapon.Id] / Weapon.NeededToBuildUnit > 1
                                 ? 1
                                 : Resources[Weapon.Id] / Weapon.NeededToBuildUnit);
            if (Armor != null)
                maxMorale *= Armor.Modifiers.MaxMoraleEfficiency *
                             (float)(Resources[Armor.Id] / Armor.NeededToBuildUnit > 1
                                 ? 1
                                 : Resources[Armor.Id] / Armor.NeededToBuildUnit);
            if (Helmet != null)
                maxMorale *= Helmet.Modifiers.MaxMoraleEfficiency *
                             (float)(Resources[Helmet.Id] / Helmet.NeededToBuildUnit > 1
                                 ? 1
                                 : Resources[Helmet.Id] / Helmet.NeededToBuildUnit);
            if (Boots != null)
                maxMorale *= Boots.Modifiers.MaxMoraleEfficiency *
                             (float)(Resources[Boots.Id] / Boots.NeededToBuildUnit > 1
                                 ? 1
                                 : Resources[Boots.Id] / Boots.NeededToBuildUnit);
            if (AdditionalSlot != null)
                maxMorale *= AdditionalSlot.Modifiers.MaxMoraleEfficiency *
                             (float)(Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit > 1
                                 ? 1
                                 : Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit);

            maxMorale *= EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.MaxMoraleEfficiency;
            maxMorale *= EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.MaxMoraleEfficiency;
            maxMorale *= EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.MaxMoraleEfficiency;

            return maxMorale;
        }
    }

    public override float MoraleIncrease
    {
        get
        {
            var moraleIncrease = 0.04f;
            if (Weapon != null)
                moraleIncrease *= Weapon.Modifiers.MoraleIncreaseEfficiency;
            if (Armor != null)
                moraleIncrease *= Armor.Modifiers.MoraleIncreaseEfficiency;
            if (Helmet != null)
                moraleIncrease *= Helmet.Modifiers.MoraleIncreaseEfficiency;
            if (Boots != null)
                moraleIncrease *= Boots.Modifiers.MoraleIncreaseEfficiency;
            if (AdditionalSlot != null)
                moraleIncrease *= AdditionalSlot.Modifiers.MoraleIncreaseEfficiency;

            moraleIncrease *= EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.MoraleIncreaseEfficiency;
            moraleIncrease *= EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.MoraleIncreaseEfficiency;
            moraleIncrease *= EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers
                .MoraleIncreaseEfficiency;

            return moraleIncrease;
        }
    }


    public override float CurrentMoraleIncrease
    {
        get
        {
            var moraleIncrease = 0.04f;
            if (Weapon != null)
                moraleIncrease *= Weapon.Modifiers.MoraleIncreaseEfficiency *
                                  (float)(Resources[Weapon.Id] / Weapon.NeededToBuildUnit > 1
                                      ? 1
                                      : Resources[Weapon.Id] / Weapon.NeededToBuildUnit);
            if (Armor != null)
                moraleIncrease *= Armor.Modifiers.MoraleIncreaseEfficiency *
                                  (float)(Resources[Armor.Id] / Armor.NeededToBuildUnit > 1
                                      ? 1
                                      : Resources[Armor.Id] / Armor.NeededToBuildUnit);
            if (Helmet != null)
                moraleIncrease *= Helmet.Modifiers.MoraleIncreaseEfficiency *
                                  (float)(Resources[Helmet.Id] / Helmet.NeededToBuildUnit > 1
                                      ? 1
                                      : Resources[Helmet.Id] / Helmet.NeededToBuildUnit);
            if (Boots != null)
                moraleIncrease *= Boots.Modifiers.MoraleIncreaseEfficiency *
                                  (float)(Resources[Boots.Id] / Boots.NeededToBuildUnit > 1
                                      ? 1
                                      : Resources[Boots.Id] / Boots.NeededToBuildUnit);
            if (AdditionalSlot != null)
                moraleIncrease *= AdditionalSlot.Modifiers.MoraleIncreaseEfficiency *
                                  (float)(Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit > 1
                                      ? 1
                                      : Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit);

            moraleIncrease *= EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.MoraleIncreaseEfficiency;
            moraleIncrease *= EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.MoraleIncreaseEfficiency;
            moraleIncrease *= EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers
                .MoraleIncreaseEfficiency;

            return moraleIncrease;
        }
    }

    public override int ManpowerGrowth
    {
        get
        {
            var manpowerGrowth = 10;
            if (Weapon != null)
                manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * Weapon.Modifiers.ManpowerIncreaseEfficiency);
            if (Armor != null)
                manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * Armor.Modifiers.ManpowerIncreaseEfficiency);
            if (Helmet != null)
                manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * Helmet.Modifiers.ManpowerIncreaseEfficiency);
            if (Boots != null)
                manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * Boots.Modifiers.ManpowerIncreaseEfficiency);
            if (AdditionalSlot != null)
                manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * AdditionalSlot.Modifiers.ManpowerIncreaseEfficiency);

            manpowerGrowth = Mathf.RoundToInt(manpowerGrowth *
                                              EngineState.MapInfo.Scenario.Countries[Owner].Modifiers
                                                  .ManpowerIncreaseEfficiency);
            manpowerGrowth = Mathf.RoundToInt(manpowerGrowth *
                                              EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas
                                                  .ManpowerIncreaseEfficiency);
            manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * EngineState.MapInfo.Scenario.Countries[Owner]
                .ConsumableGoodsModifiers.ManpowerIncreaseEfficiency);

            return manpowerGrowth;
        }
    }

    public override int CurrentManpowerGrowth
    {
        get
        {
            var manpowerGrowth = 10;
            if (Weapon != null)
                manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * Weapon.Modifiers.ManpowerIncreaseEfficiency *
                                                  (float)(Resources[Weapon.Id] / Weapon.NeededToBuildUnit > 1
                                                      ? 1
                                                      : Resources[Weapon.Id] / Weapon.NeededToBuildUnit));
            if (Armor != null)
                manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * Armor.Modifiers.ManpowerIncreaseEfficiency *
                                                  (float)(Resources[Armor.Id] / Armor.NeededToBuildUnit > 1
                                                      ? 1
                                                      : Resources[Armor.Id] / Armor.NeededToBuildUnit));
            if (Helmet != null)
                manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * Helmet.Modifiers.ManpowerIncreaseEfficiency *
                                                  (float)(Resources[Helmet.Id] / Helmet.NeededToBuildUnit > 1
                                                      ? 1
                                                      : Resources[Helmet.Id] / Helmet.NeededToBuildUnit));
            if (Boots != null)
                manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * Boots.Modifiers.ManpowerIncreaseEfficiency *
                                                  (float)(Resources[Boots.Id] / Boots.NeededToBuildUnit > 1
                                                      ? 1
                                                      : Resources[Boots.Id] / Boots.NeededToBuildUnit));
            if (AdditionalSlot != null)
                manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * AdditionalSlot.Modifiers.ManpowerIncreaseEfficiency *
                                                  (float)(Resources[AdditionalSlot.Id] /
                                                      AdditionalSlot.NeededToBuildUnit > 1
                                                          ? 1
                                                          : Resources[AdditionalSlot.Id] /
                                                            AdditionalSlot.NeededToBuildUnit));

            manpowerGrowth = Mathf.RoundToInt(manpowerGrowth *
                                              EngineState.MapInfo.Scenario.Countries[Owner].Modifiers
                                                  .ManpowerIncreaseEfficiency);
            manpowerGrowth = Mathf.RoundToInt(manpowerGrowth *
                                              EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas
                                                  .ManpowerIncreaseEfficiency);
            manpowerGrowth = Mathf.RoundToInt(manpowerGrowth * EngineState.MapInfo.Scenario.Countries[Owner]
                .ConsumableGoodsModifiers.ManpowerIncreaseEfficiency);

            return manpowerGrowth;
        }
    }

    public override int MaxManpower
    {
        get
        {
            var maxManpower = 100;
            if (Weapon != null)
                maxManpower += (int)Weapon.Modifiers.MaxManpowerBonus;
            if (Armor != null)
                maxManpower += (int)Armor.Modifiers.MaxManpowerBonus;
            if (Helmet != null)
                maxManpower += (int)Helmet.Modifiers.MaxManpowerBonus;
            if (Boots != null)
                maxManpower += (int)Boots.Modifiers.MaxManpowerBonus;
            if (AdditionalSlot != null)
                maxManpower += (int)AdditionalSlot.Modifiers.MaxManpowerBonus;

            maxManpower += (int)EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.MaxManpowerBonus;
            maxManpower += (int)EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.MaxManpowerBonus;
            maxManpower += (int)EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.MaxManpowerBonus;

            if (Weapon != null)
                maxManpower = Mathf.RoundToInt(maxManpower * Weapon.Modifiers.MaxManpowerEfficiency);
            if (Armor != null)
                maxManpower = Mathf.RoundToInt(maxManpower * Armor.Modifiers.MaxManpowerEfficiency);
            if (Helmet != null)
                maxManpower = Mathf.RoundToInt(maxManpower * Helmet.Modifiers.MaxManpowerEfficiency);
            if (Boots != null)
                maxManpower = Mathf.RoundToInt(maxManpower * Boots.Modifiers.MaxManpowerEfficiency);
            if (AdditionalSlot != null)
                maxManpower = Mathf.RoundToInt(maxManpower * AdditionalSlot.Modifiers.MaxManpowerEfficiency);

            maxManpower = Mathf.RoundToInt(maxManpower *
                                           EngineState.MapInfo.Scenario.Countries[Owner].Modifiers
                                               .MaxManpowerEfficiency);
            maxManpower = Mathf.RoundToInt(maxManpower *
                                           EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas
                                               .MaxManpowerEfficiency);
            maxManpower = Mathf.RoundToInt(maxManpower * EngineState.MapInfo.Scenario.Countries[Owner]
                .ConsumableGoodsModifiers.MaxManpowerEfficiency);

            return maxManpower;
        }
    }

    public override int CurrentMaxManpower
    {
        get
        {
            var maxManpower = 100;
            if (Weapon != null)
                maxManpower += (int)(Weapon.Modifiers.MaxManpowerBonus *
                                     (float)(Resources[Weapon.Id] / Weapon.NeededToBuildUnit > 1
                                         ? 1
                                         : Resources[Weapon.Id] / Weapon.NeededToBuildUnit));
            if (Armor != null)
                maxManpower += (int)(Armor.Modifiers.MaxManpowerBonus *
                                     (float)(Resources[Armor.Id] / Armor.NeededToBuildUnit > 1
                                         ? 1
                                         : Resources[Armor.Id] / Armor.NeededToBuildUnit));
            if (Helmet != null)
                maxManpower += (int)(Helmet.Modifiers.MaxManpowerBonus *
                                     (float)(Resources[Helmet.Id] / Helmet.NeededToBuildUnit > 1
                                         ? 1
                                         : Resources[Helmet.Id] / Helmet.NeededToBuildUnit));
            if (Boots != null)
                maxManpower += (int)(Boots.Modifiers.MaxManpowerBonus *
                                     (float)(Resources[Boots.Id] / Boots.NeededToBuildUnit > 1
                                         ? 1
                                         : Resources[Boots.Id] / Boots.NeededToBuildUnit));
            if (AdditionalSlot != null)
                maxManpower += (int)(AdditionalSlot.Modifiers.MaxManpowerBonus *
                                     (float)(Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit > 1
                                         ? 1
                                         : Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit));

            maxManpower += (int)EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.MaxManpowerBonus;
            maxManpower += (int)EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.MaxManpowerBonus;
            maxManpower += (int)EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.MaxManpowerBonus;

            if (Weapon != null)
                maxManpower = Mathf.RoundToInt(maxManpower * Weapon.Modifiers.MaxManpowerEfficiency *
                                               (float)(Resources[Weapon.Id] / Weapon.NeededToBuildUnit > 1
                                                   ? 1
                                                   : Resources[Weapon.Id] / Weapon.NeededToBuildUnit));
            if (Armor != null)
                maxManpower = Mathf.RoundToInt(maxManpower * Armor.Modifiers.MaxManpowerEfficiency *
                                               (float)(Resources[Armor.Id] / Armor.NeededToBuildUnit > 1
                                                   ? 1
                                                   : Resources[Armor.Id] / Armor.NeededToBuildUnit));
            if (Helmet != null)
                maxManpower = Mathf.RoundToInt(maxManpower * Helmet.Modifiers.MaxManpowerEfficiency *
                                               (float)(Resources[Helmet.Id] / Helmet.NeededToBuildUnit > 1
                                                   ? 1
                                                   : Resources[Helmet.Id] / Helmet.NeededToBuildUnit));
            if (Boots != null)
                maxManpower = Mathf.RoundToInt(maxManpower * Boots.Modifiers.MaxManpowerEfficiency *
                                               (float)(Resources[Boots.Id] / Boots.NeededToBuildUnit > 1
                                                   ? 1
                                                   : Resources[Boots.Id] / Boots.NeededToBuildUnit));
            if (AdditionalSlot != null)
                maxManpower = Mathf.RoundToInt(maxManpower * AdditionalSlot.Modifiers.MaxManpowerEfficiency *
                                               (float)(Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit >
                                                       1
                                                   ? 1
                                                   : Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit));

            maxManpower = Mathf.RoundToInt(maxManpower *
                                           EngineState.MapInfo.Scenario.Countries[Owner].Modifiers
                                               .MaxManpowerEfficiency);
            maxManpower = Mathf.RoundToInt(maxManpower *
                                           EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas
                                               .MaxManpowerEfficiency);
            maxManpower = Mathf.RoundToInt(maxManpower * EngineState.MapInfo.Scenario.Countries[Owner]
                .ConsumableGoodsModifiers.MaxManpowerEfficiency);

            return maxManpower;
        }
    }

    public override float Defense
    {
        get
        {
            var defense = 1f;

            if (Weapon != null)
                defense += Weapon.Modifiers.DefenseBonus*
                           (float)(Resources[Weapon.Id] / Weapon.NeededToBuildUnit > 1
                               ? 1
                               : Resources[Weapon.Id] / Weapon.NeededToBuildUnit);
            if (Armor != null)
                defense += Armor.Modifiers.DefenseBonus*
                           (float)(Resources[Armor.Id] / Armor.NeededToBuildUnit > 1
                               ? 1
                               : Resources[Armor.Id] / Armor.NeededToBuildUnit);
            if (Helmet != null)
                defense += Helmet.Modifiers.DefenseBonus*
                           (float)(Resources[Helmet.Id] / Helmet.NeededToBuildUnit > 1
                               ? 1
                               : Resources[Helmet.Id] / Helmet.NeededToBuildUnit);
            if (Boots != null)
                defense += Boots.Modifiers.DefenseBonus*
                           (float)(Resources[Boots.Id] / Boots.NeededToBuildUnit > 1
                               ? 1
                               : Resources[Boots.Id] / Boots.NeededToBuildUnit);
            if (AdditionalSlot != null)
                defense += AdditionalSlot.Modifiers.DefenseBonus*
                           (float)(Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit > 1
                               ? 1
                               : Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit);

            defense += EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.DefenseBonus;
            defense += EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.DefenseBonus;
            defense += EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.DefenseBonus;

            if (Weapon != null)
                defense *= Weapon.Modifiers.DefenseEfficiency*
                           (float)(Resources[Weapon.Id] / Weapon.NeededToBuildUnit > 1
                               ? 1
                               : Resources[Weapon.Id] / Weapon.NeededToBuildUnit);
            if (Armor != null)
                defense *= Armor.Modifiers.DefenseEfficiency*
                           (float)(Resources[Armor.Id] / Armor.NeededToBuildUnit > 1
                               ? 1
                               : Resources[Armor.Id] / Armor.NeededToBuildUnit);
            if (Helmet != null)
                defense *= Helmet.Modifiers.DefenseEfficiency*
                           (float)(Resources[Helmet.Id] / Helmet.NeededToBuildUnit > 1
                               ? 1
                               : Resources[Helmet.Id] / Helmet.NeededToBuildUnit);
            if (Boots != null)
                defense *= Boots.Modifiers.DefenseEfficiency*
                           (float)(Resources[Boots.Id] / Boots.NeededToBuildUnit > 1
                               ? 1
                               : Resources[Boots.Id] / Boots.NeededToBuildUnit);
            if (AdditionalSlot != null)
                defense *= AdditionalSlot.Modifiers.DefenseEfficiency*
                           (float)(Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit > 1
                               ? 1
                               : Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit);

            defense *= EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.DefenseEfficiency;
            defense *= EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.DefenseEfficiency;
            defense *= EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.DefenseEfficiency;

            return defense;
        } 
        
    }
    public override float MaxDefense 
    {
        get
        {
            var defense = 1f;

            if (Weapon != null)
                defense += Weapon.Modifiers.DefenseBonus;
            if (Armor != null)
                defense += Armor.Modifiers.DefenseBonus;
            if (Helmet != null)
                defense += Helmet.Modifiers.DefenseBonus;
            if (Boots != null)
                defense += Boots.Modifiers.DefenseBonus;
            if (AdditionalSlot != null)
                defense += AdditionalSlot.Modifiers.DefenseBonus;

            defense += EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.DefenseBonus;
            defense += EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.DefenseBonus;
            defense += EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.DefenseBonus;

            if (Weapon != null)
                defense *= Weapon.Modifiers.DefenseEfficiency;
            if (Armor != null)
                defense *= Armor.Modifiers.DefenseEfficiency;
            if (Helmet != null)
                defense *= Helmet.Modifiers.DefenseEfficiency;
            if (Boots != null)
                defense *= Boots.Modifiers.DefenseEfficiency;
            if (AdditionalSlot != null)
                defense *= AdditionalSlot.Modifiers.DefenseEfficiency;

            defense *= EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.DefenseEfficiency;
            defense *= EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.DefenseEfficiency;
            defense *= EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.DefenseEfficiency;

            return defense;
        } 
        
    }
    public override float Attack 
    {
        get
        {
            var attack = 1f;
            
            if (Weapon != null)
                attack += Weapon.Modifiers.AttackBonus *
                          (float)(Resources[Weapon.Id] / Weapon.NeededToBuildUnit > 1
                              ? 1
                              : Resources[Weapon.Id] / Weapon.NeededToBuildUnit);
            if (Armor != null)
                attack += Armor.Modifiers.AttackBonus *
                          (float)(Resources[Armor.Id] / Armor.NeededToBuildUnit > 1
                              ? 1
                              : Resources[Armor.Id] / Armor.NeededToBuildUnit);
            if (Helmet != null)
                attack += Helmet.Modifiers.AttackBonus *
                          (float)(Resources[Helmet.Id] / Helmet.NeededToBuildUnit > 1
                              ? 1
                              : Resources[Helmet.Id] / Helmet.NeededToBuildUnit);
            if (Boots != null)
                attack += Boots.Modifiers.AttackBonus *
                          (float)(Resources[Boots.Id] / Boots.NeededToBuildUnit > 1
                              ? 1
                              : Resources[Boots.Id] / Boots.NeededToBuildUnit);
            if (AdditionalSlot != null)
                attack += AdditionalSlot.Modifiers.AttackBonus *
                          (float)(Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit > 1
                              ? 1
                              : Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit);

            attack += EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.AttackBonus;
            attack += EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.AttackBonus;
            attack += EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.AttackBonus;

            if (Weapon != null)
                attack *= Weapon.Modifiers.AttackEfficiency *
                          (float)(Resources[Weapon.Id] / Weapon.NeededToBuildUnit > 1
                              ? 1
                              : Resources[Weapon.Id] / Weapon.NeededToBuildUnit);
            if (Armor != null)
                attack *= Armor.Modifiers.AttackEfficiency *
                          (float)(Resources[Armor.Id] / Armor.NeededToBuildUnit > 1
                              ? 1
                              : Resources[Armor.Id] / Armor.NeededToBuildUnit);
            if (Helmet != null)
                attack *= Helmet.Modifiers.AttackEfficiency *
                          (float)(Resources[Helmet.Id] / Helmet.NeededToBuildUnit > 1
                              ? 1
                              : Resources[Helmet.Id] / Helmet.NeededToBuildUnit);
            if (Boots != null)
                attack *= Boots.Modifiers.AttackEfficiency *
                          (float)(Resources[Boots.Id] / Boots.NeededToBuildUnit > 1
                              ? 1
                              : Resources[Boots.Id] / Boots.NeededToBuildUnit);
            if (AdditionalSlot != null)
                attack *= AdditionalSlot.Modifiers.AttackEfficiency *
                          (float)(Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit > 1
                              ? 1
                              : Resources[AdditionalSlot.Id] / AdditionalSlot.NeededToBuildUnit);

            attack *= EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.AttackEfficiency;
            attack *= EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.AttackEfficiency;
            attack *= EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.AttackEfficiency;

            return attack;
        } 
        
    }
    public override float MaxAttack 
    {
        get
        {
            var attack = 1f;

            if (Weapon != null)
                attack += Weapon.Modifiers.AttackBonus;
            if (Armor != null)
                attack += Armor.Modifiers.AttackBonus;
            if (Helmet != null)
                attack += Helmet.Modifiers.AttackBonus;
            if (Boots != null)
                attack += Boots.Modifiers.AttackBonus;
            if (AdditionalSlot != null)
                attack += AdditionalSlot.Modifiers.AttackBonus;

            attack += EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.AttackBonus;
            attack += EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.AttackBonus;
            attack += EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.AttackBonus;

            if (Weapon != null)
                attack *= Weapon.Modifiers.AttackEfficiency;
            if (Armor != null)
                attack *= Armor.Modifiers.AttackEfficiency;
            if (Helmet != null)
                attack *= Helmet.Modifiers.AttackEfficiency;
            if (Boots != null)
                attack *= Boots.Modifiers.AttackEfficiency;
            if (AdditionalSlot != null)
                attack *= AdditionalSlot.Modifiers.AttackEfficiency;

            attack *= EngineState.MapInfo.Scenario.Countries[Owner].Modifiers.AttackEfficiency;
            attack *= EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas.AttackEfficiency;
            attack *= EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers.AttackEfficiency;

            return attack;
        } 
        
    }

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