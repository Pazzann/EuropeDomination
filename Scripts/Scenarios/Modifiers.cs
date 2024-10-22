using System;
using System.Reflection;
using System.Text.Json.Serialization;
using EuropeDominationDemo.Scripts.GlobalStates;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

[Serializable]
public class Modifiers
{
    [JsonConstructor]
    public Modifiers()
    {
        
    }
    public Modifiers(float productionEfficiency, float transportationBonus, float additionalTrainingEfficiency, float additionalTrainingBonus, float additionalTemplateCostEfficiency, float additionalTemplateCostBonus,
        float maxMoraleBonus, float maxMoraleEfficiency, float moraleIncreaseEfficiency, int maxManpowerBonus,
        float maxManpowerEfficiency, float manpowerIncreaseEfficiency, float attackEfficiency, float attackBonus,
        float defenseEfficiency, float defenseBonus)
    {
        ProductionEfficiency = productionEfficiency;
        TransportationBonus = transportationBonus;
        AdditionalTrainingEfficiency = additionalTrainingEfficiency;
        AdditionalTrainingBonus = additionalTrainingBonus;
        AdditionalTemplateCostEfficiency = additionalTemplateCostEfficiency;
        AdditionalTemplateCostBonus = additionalTemplateCostBonus;
        MaxMoraleBonus = maxMoraleBonus;
        MaxMoraleEfficiency = maxMoraleEfficiency;
        MoraleIncreaseEfficiency = moraleIncreaseEfficiency;
        MaxManpowerBonus = maxManpowerBonus;
        MaxManpowerEfficiency = maxManpowerEfficiency;
        ManpowerIncreaseEfficiency = manpowerIncreaseEfficiency;
        AttackEfficiency = attackEfficiency;
        AttackBonus = attackBonus;
        DefenseEfficiency = defenseEfficiency;
        DefenseBonus = defenseBonus;
    }
    [HasNegativeMeaning(false)]
    public float ProductionEfficiency { get; set; }
    [HasNegativeMeaning(false)]
    public float TransportationBonus { get; set; }
    [HasNegativeMeaning(true)]
    public float AdditionalTrainingEfficiency { get; set; }
    [HasNegativeMeaning(true)]
    public float AdditionalTrainingBonus { get; set; }
    [HasNegativeMeaning(true)]
    public float AdditionalTemplateCostEfficiency { get; set; }
    [HasNegativeMeaning(true)]
    public float AdditionalTemplateCostBonus { get; set; }
    [HasNegativeMeaning(false)]
    public float MaxMoraleBonus { get; set; }
    [HasNegativeMeaning(false)]
    public float MaxMoraleEfficiency { get; set; }
    [HasNegativeMeaning(false)]
    public float MoraleIncreaseEfficiency { get; set; }
    [HasNegativeMeaning(false)]
    public float MaxManpowerBonus { get; set; }
    [HasNegativeMeaning(false)]
    public float MaxManpowerEfficiency { get; set; }
    [HasNegativeMeaning(false)]
    public float ManpowerIncreaseEfficiency { get; set; }
    [HasNegativeMeaning(false)]
    public float AttackEfficiency { get; set; }
    [HasNegativeMeaning(false)]
    public float AttackBonus { get; set; }
    [HasNegativeMeaning(false)]
    public float DefenseEfficiency { get; set; }
    [HasNegativeMeaning(false)]
    public float DefenseBonus { get; set; }

    public static Modifiers DefaultModifiers(float productionEfficiency = 1.0f, float transportationCapacity = 0.0f,
        float additionalTrainingEfficiency = 1.0f, float additionalTrainingBonus = 0f, float additionalTemplateCostEfficiency = 1.0f, float additionalTemplateCostBonus = 0f, float maxMoraleBonus = 0.0f, float maxMoraleEfficiency = 1.0f,
        float moraleIncreaseEfficiency = 1.0f,
        int maxManpowerBonus = 0, float maxManpowerEfficiency = 1.0f, float manpowerIncreaseEfficiency = 1.0f, float attackEfficiency = 1.0f, float attackBonus = 0.0f,
        float defenseEfficiency = 1.0f, float defenseBonus = 0.0f)
    {
        return new Modifiers(productionEfficiency, transportationCapacity, additionalTrainingEfficiency, additionalTrainingBonus, additionalTemplateCostEfficiency, additionalTemplateCostBonus,
            maxMoraleBonus, maxMoraleEfficiency, moraleIncreaseEfficiency, maxManpowerBonus, maxManpowerEfficiency,
            manpowerIncreaseEfficiency, attackEfficiency, attackBonus, defenseEfficiency, defenseBonus);
    }
    
    [Obsolete("Use ShowModifiers and check if resulting Text is empty")]
    public static bool IsDifferentFromDefault(Modifiers modifiers)
    {
        var defMod = DefaultModifiers();
        foreach (var propertyInfo in modifiers.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var val = propertyInfo.GetValue(modifiers);
            var defVal = propertyInfo.GetValue(defMod);
            if (Mathf.Abs((float)val - (float)defVal) > EngineVariables.Eps)
                return true;
        }

        return false;
    }

    public Modifiers ApplyModifiers(Modifiers modifiers)
    {
        foreach (var propertyInfo in modifiers.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var val = propertyInfo.GetValue(modifiers);
            propertyInfo.SetValue(this,
                propertyInfo.Name.Contains("Bonus")
                    ? ((float)propertyInfo.GetValue(this) + (float)val)
                    : ((float)propertyInfo.GetValue(this) * (float)val));
        }

        return this;
    }

    public Modifiers MultiplyByValue(float value)
    {
        foreach (var propertyInfo in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            propertyInfo.SetValue(this, (float)propertyInfo.GetValue(this) * value);
        }
        return this;
    }


    public static Modifiers operator +(Modifiers a, Modifiers b) => a.ApplyModifiers(b);
    public static Modifiers operator *(Modifiers a, float b) => a.MultiplyByValue(b);
}

[AttributeUsage(AttributeTargets.Property)]
class HasNegativeMeaning : Attribute
{
    public bool IsNegativeMeaning { get;}
    public HasNegativeMeaning(bool isNegativeMeaning) => IsNegativeMeaning = isNegativeMeaning;
}