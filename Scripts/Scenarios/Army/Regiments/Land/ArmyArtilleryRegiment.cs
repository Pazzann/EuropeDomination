using System;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;

[Serializable]
public class ArmyArtilleryRegiment : ArmyRegiment
{
    public AdditionalSlotGood AdditionalSlot{ get; set; }
    public Armor Armor{ get; set; }
    public Boots Boots{ get; set; }
    public ArtilleryWeapon Weapon{ get; set; }
    public Wheel Wheel{ get; set; }
    
    public ArmyArtilleryRegiment(string name, int owner, int templateId, int timeFromStartOfTheTraining,
        int trainingTime,
        bool isFinished, int manpower, float morale, double[] resources, BehavioralPatterns behavioralPattern,
        Modifiers modifiers) : base(name, owner, templateId, timeFromStartOfTheTraining, trainingTime, isFinished,
        manpower, morale, resources, behavioralPattern, modifiers)
    {
    }
    [JsonConstructor]
    public ArmyArtilleryRegiment()
    {
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