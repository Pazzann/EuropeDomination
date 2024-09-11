using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Naval;

public class HeavyShip : Ship
{
    public HeavyShip(string name, int owner, int templateId, int timeFromStartOfTheTraining, int trainingTime,
        bool isFinished, int manpower, float morale, double[] resources, BehavioralPatterns behavioralPattern,
        Modifiers modifiers) : base(name, owner, templateId, timeFromStartOfTheTraining, trainingTime, isFinished,
        manpower,
        morale, resources, behavioralPattern, modifiers)
    {
    }

    public override float MaxMorale { get; }
    public override float MoraleIncrease { get; }
    public override float CombatReadiness { get; }
    public override int ManpowerGrowth { get; }
    public override int MaxManpower { get; }
    public override float CombatAbility { get; }
    public override float Defense { get; }
    public override float MovementSpeed { get; }
    public override float SurvivalIndex { get; }

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