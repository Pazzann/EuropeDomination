using System;
using System.Reflection;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;
using EuropeDominationDemo.Scripts.Utils;
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
    
    [JsonConstructor]
    public ArmyInfantryRegiment()
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