using System;
using System.Text.Json.Serialization;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;

[Serializable]
public class Weapon : Good
{
    public Weapon(int id, string name, Vector3 color, float cost, float battleConsumption, float walkingConsumption,
        float steadyConsumption, float neededToBuildUnit, Modifiers modifiers, int additionalTrainingTime) : base(id,
        name, color, cost)
    {
        BattleConsumption = battleConsumption;
        WalkingConsumption = walkingConsumption;
        SteadyConsumption = steadyConsumption;
        NeededToBuildUnit = neededToBuildUnit;
        Modifiers = modifiers;
        AdditionalTrainingTime = additionalTrainingTime;
    }
    [JsonConstructor]
    public Weapon()
    {
        
    }

    public float BattleConsumption { get; init; }
    public float WalkingConsumption { get;init;  }
    public float SteadyConsumption { get;init;  }
    public float NeededToBuildUnit { get;init;  }
    public Modifiers Modifiers { get;init;  }

    public int AdditionalTrainingTime { get; init; }
}