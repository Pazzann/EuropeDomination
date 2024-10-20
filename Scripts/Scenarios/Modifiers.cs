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
    public Modifiers(float productionEfficiency, float transportationBonus, float additionalTrainingEfficiency,
        float maxMoraleBonus, float maxMoraleEfficiency, float moraleIncreaseEfficiency, int maxManpowerBonus,
        float maxManpowerEfficiency, float manpowerIncreaseEfficiency, float attackEfficiency, float attackBonus,
        float defenseEfficiency, float defenseBonus)
    {
        ProductionEfficiency = productionEfficiency;
        TransportationBonus = transportationBonus;
        AdditionalTrainingEfficiency = additionalTrainingEfficiency;
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

    public float ProductionEfficiency { get; set; }
    public float TransportationBonus { get; set; }
    public float AdditionalTrainingEfficiency { get; set; }
    public float MaxMoraleBonus { get; set; }
    public float MaxMoraleEfficiency { get; set; }
    public float MoraleIncreaseEfficiency { get; set; }
    public float MaxManpowerBonus { get; set; }
    public float MaxManpowerEfficiency { get; set; }
    public float ManpowerIncreaseEfficiency { get; set; }
    public float AttackEfficiency { get; set; }
    public float AttackBonus { get; set; }
    public float DefenseEfficiency { get; set; }
    public float DefenseBonus { get; set; }


    public static Modifiers DefaultModifiers(float productionEfficiency = 1.0f, float transportationCapacity = 0.0f,
        float additionalTrainingEfficiency = 1.0f, float maxMoraleBonus = 0.0f, float maxMoraleEfficiency = 1.0f,
        float moraleIncreaseEfficiency = 1.0f,
        int maxManpowerBonus = 0, float maxManpowerEfficiency = 1.0f, float manpowerIncreaseEfficiency = 1.0f, float attackEfficiency = 1.0f, float attackBonus = 0.0f,
        float defenseEfficiency = 1.0f, float defenseBonus = 0.0f)
    {
        return new Modifiers(productionEfficiency, transportationCapacity, additionalTrainingEfficiency,
            maxMoraleBonus, maxMoraleEfficiency, moraleIncreaseEfficiency, maxManpowerBonus, maxManpowerEfficiency,
            manpowerIncreaseEfficiency, attackEfficiency, attackBonus, defenseEfficiency, defenseBonus);
    }

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

    public void ApplyModifiers(Modifiers modifiers)
    {
        foreach (var propertyInfo in modifiers.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var val = propertyInfo.GetValue(modifiers);
            propertyInfo.SetValue(this,
                propertyInfo.Name.Contains("Bonus")
                    ? ((float)propertyInfo.GetValue(this) + (float)val)
                    : ((float)propertyInfo.GetValue(this) * (float)val));
        }
    }
}