using System;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;

[Serializable]
[JsonDerivedType(typeof(ArmyInfantryRegiment), typeDiscriminator: "armyInfantryRegiment")]
[JsonDerivedType(typeof(ArmyArtilleryRegiment), typeDiscriminator: "armyArtilleryRegiment")]
[JsonDerivedType(typeof(ArmyCavalryRegiment), typeDiscriminator: "armyCavalryRegiment")]

[JsonDerivedType(typeof(TransportShip), typeDiscriminator: "transportShip")]
[JsonDerivedType(typeof(LightShip), typeDiscriminator: "lightShip")]
[JsonDerivedType(typeof(HeavyShip), typeDiscriminator: "heavyShip")]
[JsonDerivedType(typeof(MediumShip), typeDiscriminator: "medicalShip")]
public abstract class Regiment
{
    public BehavioralPatterns BehavioralPattern { get; set; }
    public bool IsFinished { get; set; }

    public int Manpower { get; set; }
    public Modifiers Modifiers { get; set; }

    public float Morale { get; set; }
    public string Name { get; set; }
    public int Owner { get; set; }
    public double[] Resources { get; set; }
    public int TemplateId { get; set; }

    public int TimeFromStartOfTheTraining { get; set; }
    public int TrainingTime { get; set; }

    public Regiment(string name, int owner, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, float morale, double[] resources, BehavioralPatterns behavioralPattern,
        Modifiers modifiers)
    {
        Name = name;
        Owner = owner;
        TemplateId = templateId;


        TimeFromStartOfTheTraining = timeFromStartOfTheTraining;
        TrainingTime = trainingTime;
        IsFinished = isFinished;

        Manpower = manpower;
        Morale = morale;
        Resources = resources;

        BehavioralPattern = behavioralPattern;
        Modifiers = modifiers;
    }


    public abstract float MaxMorale { get; }
    public abstract float CurrentMaxMorale { get; }
    public abstract float MoraleIncrease { get; }
    public abstract float CurrentMoraleIncrease { get; }
    public abstract int ManpowerGrowth { get; }
    public abstract int CurrentManpowerGrowth { get; }
    public abstract int MaxManpower { get; }
    public abstract int CurrentMaxManpower { get; }
    public abstract float Defense { get; }
    public abstract float MaxDefense { get; }
    public abstract float Attack { get; }
    public abstract float MaxAttack { get; }
    public abstract void Consume();
    public abstract void Recalculate();
    public abstract void ChangeTemplate();

    public bool DayTick()
    {
        TimeFromStartOfTheTraining++;
        return TimeFromStartOfTheTraining >= TrainingTime;
    }
}