using System;
using System.Reflection;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;
using Godot;

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

    [JsonConstructor]
    public Regiment()
    {
    }
    
    
    public float MaxModifier(string propertyName, float defVal)
    {
            var combinedMaxModifiers = CombineMaxModifiers();
            return (defVal + (float)combinedMaxModifiers.GetType().GetProperty(propertyName + "Bonus").GetValue(combinedMaxModifiers)) * (float)combinedMaxModifiers.GetType().GetProperty(propertyName + "Efficiency").GetValue(combinedMaxModifiers);
    }
    
    public float CurrentMaxModifier(string propertyName, float defVal)
    {
        var combinedMaxModifiers = CombineModifiers();
        return (defVal + (float)combinedMaxModifiers.GetType().GetProperty(propertyName + "Bonus").GetValue(combinedMaxModifiers)) * (float)combinedMaxModifiers.GetType().GetProperty(propertyName + "Efficiency").GetValue(combinedMaxModifiers);
    }

    public float MaxMorale => MaxModifier("MaxMorale", 1f);
    public float CurrentMaxMorale => CurrentMaxModifier("MaxMorale", 1f);
     
    public float MoraleIncrease => MaxModifier("MoraleIncrease", 1f);
    public float CurrentMoraleIncrease => CurrentMaxModifier("MoraleIncrease", 1f);
    public int ManpowerGrowth => Mathf.RoundToInt(MaxModifier("ManpowerGrowth", 100f));
    public int CurrentManpowerGrowth => Mathf.RoundToInt(CurrentMaxModifier("ManpowerGrowth", 100f));
    public int MaxManpower => Mathf.RoundToInt(MaxModifier("MaxManpower", 100f));
    public int CurrentMaxManpower => Mathf.RoundToInt(CurrentMaxModifier("MaxManpower", 100f));
    public float Defense => CurrentMaxModifier("Defense", 1f);
    public float MaxDefense => MaxModifier("Defense", 1f);
    public float Attack => CurrentMaxModifier("Attack", 1f);
    public float MaxAttack => MaxModifier("Attack", 1f);

    public int Range => 2;
    public int Step => 1;

    #nullable enable
    public Regiment? Target { get; set; }
    public Vector2I? Position { get; set; }
    
    
    // todo: for future updates movement logic is different for each regiment type
    
    
    public void Move()
    {
        
    }

    public void AttackTarget()
    {
        
    }

    public int DistanceTo(Vector2I? destination)
    {
        if (destination is Vector2I nonNullDestination && Position is Vector2I nonNullPosition)
            return Mathf.Max(Mathf.Abs(nonNullDestination.X - nonNullPosition.X),
                Mathf.Abs(nonNullDestination.Y - nonNullPosition.Y));
        else return -1;
    }
    
    public abstract void Consume();
    public abstract void Recalculate();
    public abstract void ChangeTemplate();
    
    
    public Modifiers CombineModifiers()
    {
        var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        
        var modifiers = Modifiers.DefaultModifiers();
        modifiers += Modifiers;
        
        for (int i = 0; i < 5; i++)
        {
            if (properties[i].GetValue(this) != null)
                modifiers += (Modifiers)properties[i].GetType().GetProperty("Modifiers").GetValue(properties[i].GetValue(this))
                             * (float)Mathf.Min(Resources[(int)properties[i].GetType().GetProperty("Id").GetValue(properties[i].GetValue(this))] / 
                                                (float)properties[i].GetType().GetProperty("NeededToBuildUnit").GetValue(properties[i].GetValue(this)), 1);
        }

        modifiers += EngineState.MapInfo.Scenario.Countries[Owner].Modifiers;
        modifiers += EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas;
        modifiers += EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers;
        
        return modifiers;
    }
    
    public Modifiers CombineMaxModifiers()
    {
        var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        
        var modifiers = Modifiers.DefaultModifiers();
        modifiers += Modifiers;
        
        for (int i = 0; i < 5; i++)
        {
            if (properties[i].GetValue(this) != null)
                modifiers += (Modifiers)properties[i].GetType().GetProperty("Modifiers").GetValue(properties[i].GetValue(this));
        }

        modifiers += EngineState.MapInfo.Scenario.Countries[Owner].Modifiers;
        modifiers += EngineState.MapInfo.Scenario.Countries[Owner].NationalIdeas;
        modifiers += EngineState.MapInfo.Scenario.Countries[Owner].ConsumableGoodsModifiers;
        
        return modifiers;
    }

    public bool DayTick()
    {
        TimeFromStartOfTheTraining++;
        return TimeFromStartOfTheTraining >= TrainingTime;
    }
    
    
}