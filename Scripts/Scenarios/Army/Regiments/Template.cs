using System;
using System.Reflection;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;

[Serializable]
[JsonDerivedType(typeof(ArmyInfantryRegimentTemplate), typeDiscriminator: "armyInfantryRegimentTemplate")]
[JsonDerivedType(typeof(ArmyArtilleryRegimentTemplate), typeDiscriminator: "armyArtilleryRegimentTemplate")]
[JsonDerivedType(typeof(ArmyCavalryRegimentTemplate), typeDiscriminator: "armyCavalryRegimentTemplate")]

[JsonDerivedType(typeof(TransportShipTemplate), typeDiscriminator: "transportShipTemplate")]
[JsonDerivedType(typeof(LightShipTemplate), typeDiscriminator: "lightShipTemplate")]
[JsonDerivedType(typeof(HeavyShipTemplate), typeDiscriminator: "heavyShipTemplate")]
[JsonDerivedType(typeof(MediumShipTemplate), typeDiscriminator: "medicalShipTemplate")]
public abstract class Template
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Owner { get; set; }

    public Template(string name, int id, int owner)
    {
        Name = name;
        Id = id;
        Owner = owner;
    }
    [JsonConstructor]
    public Template()
    {
    }

    public int TrainingTime => Mathf.RoundToInt(MaxModifier("AdditionalTraining", 10));
    public float Cost => MaxModifier("AdditionalTemplateCost", 10);
    
    public float MaxModifier(string propertyName, float defVal)
    {
        var combinedMaxModifiers = CombineMaxModifiers();
        return (defVal + (float)combinedMaxModifiers.GetType().GetProperty(propertyName + "Bonus")!.GetValue(combinedMaxModifiers)!) * (float)combinedMaxModifiers.GetType().GetProperty(propertyName + "Efficiency")!.GetValue(combinedMaxModifiers)!;
    }
    public Modifiers CombineMaxModifiers()
    {
        var properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        
        var modifiers = Modifiers.DefaultModifiers();
        
        for (int i = 0; i < 5; i++)
        {
            if (properties[i].GetValue(this) != null)
                modifiers += (Modifiers)properties[i].GetType().GetProperty("Modifiers")!.GetValue(properties[i].GetValue(this));
        }

        modifiers += EngineState.MapInfo.Scenario.Countries[Owner].TotalModifiers;
        
        return modifiers;
    }
    
}