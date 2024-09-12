﻿using System.Reflection;
using EuropeDominationDemo.Scripts.GlobalStates;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class Modifiers
{
    public Modifiers(float productionEfficiency, float transportationBonus, float additionalTrainingEfficiency,
        float maxMoraleBonus, float maxMoraleEfficiency, float moraleIncreaseEfficiency, int maxManpowerBonus,
        float maxManpowerEfficiency, float manpowerIncreaseEfficiency)
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


    public static Modifiers DefaultModifiers(float productionEfficiency = 1.0f, float transportationCapacity = 0.0f,
        float additionalTrainingEfficiency = 1.0f, float maxMoraleBonus = 0.0f, float maxMoraleEfficiency = 1.0f,
        float moraleIncreaseEfficiency = 1.0f,
        int maxManpowerBonus = 0, float maxManpowerEfficiency = 1.0f, float manpowerIncreaseEfficiency = 1.0f)
    {
        return new Modifiers(productionEfficiency, transportationCapacity, additionalTrainingEfficiency,
            maxMoraleBonus, maxMoraleEfficiency, moraleIncreaseEfficiency, maxManpowerBonus, maxManpowerEfficiency,
            manpowerIncreaseEfficiency);
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
            propertyInfo.SetValue(this, propertyInfo.Name.Contains("Bonus") ? ((float)propertyInfo.GetValue(this) + (float)val): ((float)propertyInfo.GetValue(this) * (float)val));
        }
    }
}